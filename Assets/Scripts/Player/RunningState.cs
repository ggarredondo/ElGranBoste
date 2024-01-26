using UnityEngine;

[System.Serializable]
public class RunningState : PlayerState
{
    [SerializeField] private float movementSpeed = 1f;
    public void Initialize(in PlayerStateMachine player) => base.Initialize("MOVEMENT", player);

    public override void Enter()
    {
        player.InputController.OnPressJoke += player.TransitionToJoking;
        base.Enter();
    }
    public override void Update()
    {
        player.Move(movementSpeed);
        player.Fall();
    }
    public override void Exit()
    {
        player.InputController.OnPressJoke -= player.TransitionToJoking;
        base.Exit();
    }
}
