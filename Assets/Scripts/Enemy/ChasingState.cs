using UnityEngine;

[System.Serializable]
public class ChasingState : EnemyState
{
    public void Initialize(in EnemyStateMachine enemy) => base.Initialize("CHASING", enemy);

    public override void Enter()
    {
        enemy.PlayerToEnemyEvents.OnJokePerformed += ReceiveJoke;
        base.Enter();
    }
    public override void Update()
    {
        enemy.FollowPlayer();
    }
    public override void Exit() 
    { 
        enemy.PlayerToEnemyEvents.OnJokePerformed -= ReceiveJoke;
        base.Exit();
    }

    public void ReceiveJoke(in Joke joke)
    {
        if (joke.Type == JokeType.Stun) enemy.TransitionToLaughing(joke);
        else if (joke.Type == JokeType.Poste) enemy.TransitionToPoste();
    }
}
