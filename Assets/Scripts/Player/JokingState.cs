using UnityEngine;
using DG.Tweening;

[System.Serializable]
public class JokingState : PlayerState
{
    [SerializeField] private float movementSpeed = 1f;
    private Sequence sequence;
    private Joke currentJoke;

    public System.Action OnJokePerformed;
    public void Initialize(in PlayerStateMachine player) => base.Initialize("JOKING", player);

    public override void Enter()
    { 
        currentJoke = player.JokeList[player.SelectedJoke];
        sequence = DOTween.Sequence();
        sequence.AppendInterval(currentJoke.TimeToPerform).OnComplete(PerformJoke);
        player.InputController.OnReleaseJoke += player.TransitionToRunning;
        player.InputController.OnPressParry += player.TransitionToParry;
        player.PlayerToEnemyEvents.OnKillPlayer += player.TransitionToDead;
        base.Enter();
    }
    public override void Update()
    {
        player.PlayerToEnemyEvents.OnJokeStart?.Invoke();
        player.Move(movementSpeed);
        player.LookForward();
        player.Fall();
        if (!player.IsEnemyInCameraView())
            player.TransitionToRunning();
    }
    public override void Exit()
    {
        player.PlayerToEnemyEvents.OnJokeCancelled?.Invoke();
        sequence.Kill();
        player.InputController.OnReleaseJoke -= player.TransitionToRunning;
        player.InputController.OnPressParry -= player.TransitionToParry;
        player.PlayerToEnemyEvents.OnKillPlayer -= player.TransitionToDead;
        base.Exit();
    }

    public void PerformJoke()
    {
        player.PlayerToEnemyEvents.OnJokePerformed?.Invoke(currentJoke);
        OnJokePerformed?.Invoke();
        player.TransitionToRunning();
    }

    public float MovementSpeed => movementSpeed;
}
