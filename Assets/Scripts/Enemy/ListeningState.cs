using UnityEngine;

[System.Serializable]
public class ListeningState : EnemyState
{
    [SerializeField] private float movementSpeed = 1f;
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("LISTENING", enemy);

    public override void Enter()
    {
        enemy.Agent.speed = movementSpeed;
        enemy.PlayerToEnemyEvents.OnJokePerformed += ReceiveJoke;
        enemy.PlayerToEnemyEvents.OnJokeCancelled += enemy.TransitionToChasing;
        base.Enter();
    }
    public override void Update()
    {
        enemy.FollowPlayer();
    }
    public override void Exit()
    {
        enemy.PlayerToEnemyEvents.OnJokePerformed -= ReceiveJoke;
        enemy.PlayerToEnemyEvents.OnJokeCancelled -= enemy.TransitionToChasing;
        base.Exit();
    }

    private void ReceiveJoke(in Joke joke)
    {
        if (joke.Type == JokeType.Stun) enemy.TransitionToLaughing(joke);
        else if (joke.Type == JokeType.Poste) enemy.TransitionToPoste();
    }
}
