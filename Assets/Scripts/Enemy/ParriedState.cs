using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class ParriedState : EnemyState
{
    [SerializeField] private float parriedTime;
    private Sequence sequence;
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("PARRIED", enemy);

    public override void Enter()
    {
        sequence = DOTween.Sequence();
        sequence.AppendInterval(parriedTime).OnComplete(enemy.TransitionToChasing);
        enemy.PlayerToEnemyEvents.OnJokePerformed += ReceiveJoke;
        base.Enter();
    }
    public override void Update() {}
    public override void Exit()
    {
        enemy.PlayerToEnemyEvents.OnJokePerformed -= ReceiveJoke;
        sequence.Kill();
        base.Exit();
    }

    private void Test() => Debug.Log("cancelled");

    private void ReceiveJoke(in Joke joke)
    {
        if (joke.Type == JokeType.Stun) enemy.TransitionToLaughing(joke);
        else if (joke.Type == JokeType.Poste) enemy.TransitionToPoste();
    }
}
