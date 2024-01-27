using UnityEngine;

[System.Serializable]
public class ChasingState : EnemyState
{
    [SerializeField] private float movementSpeed = 1f, minDistanceToListen;
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("CHASING", enemy);

    public override void Enter()
    {
        enemy.Agent.speed = movementSpeed;
        enemy.PlayerToEnemyEvents.OnJokeStart += enemy.TransitionToListening;
        base.Enter();
    }
    public override void Update()
    {
        enemy.FollowPlayer();
    }
    public override void Exit() 
    {
        enemy.PlayerToEnemyEvents.OnJokeStart -= enemy.TransitionToListening;
        base.Exit();
    }
}
