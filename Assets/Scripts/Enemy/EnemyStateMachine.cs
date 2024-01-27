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
    [SerializeField] private ListeningState listeningState;
    [SerializeField] private LaughingState laughingState;
    [SerializeField] private PosteState posteState;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        chasingState.Initialize(this);
        listeningState.Initialize(this);
        laughingState.Initialize(this);
        posteState.Initialize(this);
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
    public void Fall() => agent.Move(Physics.gravity * Time.deltaTime);
    public void StopFollowing() => agent.SetDestination(transform.position);
    public float DistanceToPlayer => Vector3.Distance(transform.position, target.position);

    public void TransitionToChasing() => ChangeState(chasingState);
    public void TransitionToListening() => ChangeState(listeningState);
    public void TransitionToLaughing(in Joke joke)
    {
        laughingState.SetJoke(joke);
        ChangeState(laughingState);
    }
    public void TransitionToPoste() => ChangeState(posteState);

    // Gets
    public ref readonly ChasingState ChasingState => ref chasingState;
    public ref readonly ListeningState ListeningState => ref listeningState;
    public ref readonly LaughingState LaughingState => ref laughingState;
    public ref readonly PosteState PosteState => ref posteState;

    public ref readonly NavMeshAgent Agent => ref agent;
    public ref readonly PlayerToEnemyEvents PlayerToEnemyEvents => ref playerToEnemyEvents;
}
