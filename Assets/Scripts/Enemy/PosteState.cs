
[System.Serializable]
public class PosteState : EnemyState
{
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("POSTE", enemy);

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
