using UnityEngine;
using UnityEngine.AI;

public class EnemyStateMachine : MonoBehaviour
{
    private NavMeshAgent agent;
    private Transform target;
    private PlayerToEnemyEvents playerToEnemyEvents;

    [SerializeField][ReadOnlyField] private string currentStateName;
    private EnemyState currentState;
    [SerializeField] private ChasingState chasingState;
    [SerializeField] private LaughingState laughingState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        chasingState.Initialize(this);
        laughingState.Initialize(this);
    }
    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        playerToEnemyEvents = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerToEnemyEvents>();
        ChangeState(chasingState);
    }
    private void Update() => currentState.Update();

    private void ChangeState(in EnemyState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentStateName = newState.StateName;
        currentState.Enter();
    }

    // API
    public void Enable(bool enabled) => this.enabled = enabled;
    public void FollowPlayer() => agent.SetDestination(target.position);
    public void StopFollowing() => agent.SetDestination(transform.position);

    public void TransitionToChasing() => ChangeState(chasingState);
    public void TransitionToLaughing() => ChangeState(laughingState);

    // Gets
    public ref readonly PlayerToEnemyEvents PlayerToEnemyEvents => ref playerToEnemyEvents;
}
