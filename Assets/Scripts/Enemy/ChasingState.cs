using UnityEngine;

[System.Serializable]
public class ChasingState : EnemyState
{
    [SerializeField] private float movementSpeed = 1f;
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("CHASING", enemy);

    public override void Enter()
    {
        enemy.Agent.speed = movementSpeed;
        enemy.PlayerToEnemyEvents.OnJokeStart += enemy.TransitionToListening;
        enemy.PlayerToEnemyEvents.OnParry += enemy.TransitionToParried;
        base.Enter();
    }
    public override void Update()
    {
        enemy.FollowPlayer();
        enemy.KillPlayer();
    }
    public override void Exit() 
    {
        enemy.PlayerToEnemyEvents.OnJokeStart -= enemy.TransitionToListening;
        enemy.PlayerToEnemyEvents.OnParry -= enemy.TransitionToParried;
        base.Exit();
    }
}
