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
    [SerializeField] private PlayerAnimation playerAnimation;
    [SerializeField] private List<Joke> jokeList;
    private int selectedJoke = 0;

    [SerializeField][ReadOnlyField] private string currentStateName;
    private PlayerState currentState;
    [SerializeField] private RunningState runningState;
    [SerializeField] private JokingState jokingState;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        cam = Camera.main;
        enemyTransform = GameObject.FindGameObjectWithTag("Enemy").transform;
        playerToEnemyEvents = GetComponent<PlayerToEnemyEvents>();
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<InputController>();
        inputController.Initialize();
        velocity = Vector3.zero;

        runningState.Initialize(this);
        jokingState.Initialize(this);
        ChangeState(runningState);

        playerAnimation.Initialize(this, GetComponent<Animator>());
    }
    private void Start() {}
    private void Update() => currentState.Update();

    private void ChangeState(in PlayerState newState)
    {
        if (currentState != null) currentState.Exit();
        currentState = newState;
        currentStateName = newState.StateName;
        currentState.Enter();
    }

    // API
    public void Enable(bool enabled) => this.enabled = enabled;
    public void Move(float movementSpeed)
    {
        Vector3 forward = Vector3.Cross(cam.transform.right, Vector3.up).normalized;
        Vector3 direction = cam.transform.right * inputController.MovementDirection.x 
            + forward * inputController.MovementDirection.y;
        transform.LookAt(transform.position + cam.transform.forward);
        velocity = direction * movementSpeed;
        characterController.Move(velocity * Time.deltaTime);
    }
    public void Fall() => characterController.Move(Physics.gravity * Time.deltaTime);
    public bool IsEnemyInCameraView()
    {
        Vector3 viewPos = cam.WorldToViewportPoint(enemyTransform.position);
        return viewPos.x > 0f && viewPos.x < 1f && viewPos.y > 0f && viewPos.y < 1f && viewPos.z > 0f;
    }
    public void SetSelectedJoke(int index) => selectedJoke = index;

    public void TransitionToRunning() => ChangeState(runningState);
    public void TransitionToJoking() => ChangeState(jokingState);

    // Gets
    public ref readonly RunningState RunningState => ref runningState;
    public ref readonly JokingState JokingState => ref jokingState;

    public ref readonly PlayerToEnemyEvents PlayerToEnemyEvents => ref playerToEnemyEvents;
    public ref readonly CharacterController CharacterController => ref characterController;
    public ref readonly InputController InputController => ref inputController;
    public ref readonly Vector3 Velocity => ref velocity;
    public ref readonly List<Joke> JokeList => ref jokeList;
    public int SelectedJoke => selectedJoke;
}
