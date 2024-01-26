using UnityEngine;

[System.Serializable]
public class LaughingState : EnemyState
{
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("LAUGHING", enemy);

    public override void Enter()
    {
        enemy.StopFollowing();
        base.Enter();
    }
    public override void Update() {}
    public override void Exit()
    {
        base.Exit();
    }
}
