using DG.Tweening;
using UnityEngine;

[System.Serializable]
public class LaughingState : EnemyState
{
    private Sequence sequence;
    private Joke receivedJoke;
    public void SetJoke(in Joke joke) => receivedJoke = joke;
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("LAUGHING", enemy);

    public override void Enter()
    {
        enemy.PlayerToEnemyEvents.OnJokePerformed += ReceiveJoke;
        sequence = DOTween.Sequence();
        sequence.AppendInterval(receivedJoke.LaughingTime + enemy.LaughingExtraTime).OnComplete(enemy.TransitionToChasing);
        enemy.StopFollowing();
        base.Enter();
    }
    public override void Update() {}
    public override void Exit()
    {
        sequence.Kill();
        enemy.PlayerToEnemyEvents.OnJokePerformed -= ReceiveJoke;
        base.Exit();
    }

    private void ReceiveJoke(in Joke joke)
    {
        if (joke.Type == JokeType.Stun) enemy.TransitionToLaughing(joke);
        else if (joke.Type == JokeType.Poste) enemy.TransitionToPoste();
    }

    public ref readonly Joke Joke => ref receivedJoke;
}
