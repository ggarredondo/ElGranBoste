using UnityEngine;

[System.Serializable]
public class RunningState : PlayerState
{
    [SerializeField] private float movementSpeed = 1f;
    public void Initialize(in PlayerStateMachine player) => base.Initialize("RUNNING", player);

    public override void Enter()
    {
        player.InputController.OnPressJoke += JokeStart;
        player.InputController.OnPressParry += player.TransitionToParry;
        player.PlayerToEnemyEvents.OnKillPlayer += player.TransitionToDead;
        base.Enter();
    }
    public override void Update()
    {
        player.Move(movementSpeed);
        player.LookForward();
        player.Fall();
    }
    public override void Exit()
    {
        player.InputController.OnPressJoke -= JokeStart;
        player.InputController.OnPressParry -= player.TransitionToParry;
        player.PlayerToEnemyEvents.OnKillPlayer -= player.TransitionToDead;
        base.Exit();
    }

    public void JokeStart()
    {
        if (player.IsEnemyInCameraView())
            player.TransitionToJoking();
    }
    public float MovementSpeed => movementSpeed;
}
