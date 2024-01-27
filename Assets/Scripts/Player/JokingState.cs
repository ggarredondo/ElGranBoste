using UnityEngine;

[System.Serializable]
public class JokingState : PlayerState
{
    [SerializeField] private float movementSpeed = 1f;
    public void Initialize(in PlayerStateMachine player) => base.Initialize("JOKING", player);

    public override void Enter()
    {
        player.InputController.OnReleaseJoke += player.TransitionToRunning;
        base.Enter();
    }
    public override void Update()
    {
        player.Move(movementSpeed);
        player.Fall();
    }
    public override void Exit()
    {
        if (player.IsEnemyInCameraView()) player.PlayerToEnemyEvents.OnJokePerformed?.Invoke();
        player.InputController.OnReleaseJoke -= player.TransitionToRunning;
        base.Exit();
    }
}
