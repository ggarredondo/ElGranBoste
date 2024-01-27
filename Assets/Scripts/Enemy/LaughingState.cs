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
        sequence = DOTween.Sequence();
        sequence.AppendInterval(receivedJoke.LaughingTime).OnComplete(enemy.TransitionToChasing);
        enemy.StopFollowing();
        base.Enter();
    }
    public override void Update() {}
    public override void Exit()
    {
        base.Exit();
    }
}
