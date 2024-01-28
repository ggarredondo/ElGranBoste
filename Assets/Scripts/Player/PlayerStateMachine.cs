using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Camera cam;
    private Transform enemyTransform;
    private PlayerToEnemyEvents playerToEnemyEvents;
    private CharacterController characterController;
    private InputController inputController;
    private Vector3 velocity;
    [SerializeField] private List<Joke> jokeList;
    private int selectedJoke = 0;

    [SerializeField][ReadOnlyField] private string currentStateName;
    private PlayerState currentState;
    [SerializeField] private RunningState runningState;
    [SerializeField] private JokingState jokingState;
    [SerializeField] private DeadState deadState;
    [SerializeField] private ParryState parryState;
    public bool disableTransitions;

    public System.Action OnPickBook;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
        playerToEnemyEvents = GetComponent<PlayerToEnemyEvents>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<InputController>();
        velocity = Vector3.zero;

        disableTransitions = false;
        runningState.Initialize(this);
        jokingState.Initialize(this);
        deadState.Initialize(this);
        parryState.Initialize(this);
    }
    private void Start()
    {
        cam = Camera.main;
        ChangeState(runningState);
        InitializeAllJokes();
    }
    private void Update()
    {
        currentState.Update();
    }

    private void ChangeState(in PlayerState newState)
    {
        if (!disableTransitions)
        {
            if (currentState != null) currentState.Exit();
            currentState = newState;
            currentStateName = newState.StateName;
            currentState.Enter();
        }
    }

    // API
    public void Enable(bool enabled) => this.enabled = enabled;
    public void Move(float movementSpeed)
    {
        Vector3 forward = Vector3.Cross(cam.transform.right, Vector3.up).normalized;
        Vector3 direction = cam.transform.right * inputController.MovementDirection.x 
            + forward * inputController.MovementDirection.y;
        velocity = direction * movementSpeed;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void ResetMoveVector() => velocity = Vector3.zero;
    public void LookForward() => transform.LookAt(transform.position + cam.transform.forward);
    public void Fall() => characterController.Move(Physics.gravity * Time.deltaTime);
    public bool IsEnemyInCameraView()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(enemyTransform.position);
        return viewPos.x >= 0f && viewPos.x <= 1f && viewPos.y >= 0f && viewPos.y <= 1f && viewPos.z > 0f;
    }
    public void SetSelectedJoke(int index) => selectedJoke = index;
    public void InitializeAllJokes()
    {
        for (int i = 0; i < jokeList.Count; ++i)
            jokeList[i].Initialize();
    }
    public float DistanceToEnemy => Vector3.Distance(transform.position, enemyTransform.position);
    public void TransitionToRunning() => ChangeState(runningState);
    public void TransitionToJoking()
    {
        if(jokeList.Count != 0)
            ChangeState(jokingState);
    }
    public void TransitionToParry() => ChangeState(parryState);
    public void TransitionToDead() => ChangeState(deadState);

    // Gets
    public ref readonly RunningState RunningState => ref runningState;
    public ref readonly JokingState JokingState => ref jokingState;
    public ref readonly ParryState ParryState => ref parryState;
    public ref readonly DeadState DeadState => ref deadState;

    public ref readonly PlayerToEnemyEvents PlayerToEnemyEvents => ref playerToEnemyEvents;
    public ref readonly CharacterController CharacterController => ref characterController;
    public ref readonly InputController InputController => ref inputController;
    public ref readonly Vector3 Velocity => ref velocity;
    public ref readonly List<Joke> JokeList => ref jokeList;
    public int SelectedJoke => selectedJoke;
}
