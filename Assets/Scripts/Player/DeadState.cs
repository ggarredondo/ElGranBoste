using UnityEngine;

[System.Serializable]
public class DeadState : PlayerState
{
    public void Initialize(in PlayerStateMachine player) => base.Initialize("DEAD", player);

    public override void Enter()
    {
        player.PlayerToEnemyEvents.OnParry = null;
        player.disableTransitions = true;
        base.Enter();
    }
    public override void Update()
    {
        player.LookForward();
    }
    public override void Exit()
    {
        base.Exit();
    }
}
