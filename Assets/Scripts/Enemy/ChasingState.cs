using UnityEngine;

[System.Serializable]
public class ChasingState : EnemyState
{
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("CHASING", enemy);

    public override void Enter()
    {
        enemy.PlayerToEnemyEvents.OnJokePerformed += enemy.TransitionToLaughing;
        base.Enter();
    }
    public override void Update()
    {
        enemy.FollowPlayer();
    }
    public override void Exit() 
    { 
        enemy.PlayerToEnemyEvents.OnJokePerformed -= enemy.TransitionToLaughing;
        base.Exit();
    }
}
