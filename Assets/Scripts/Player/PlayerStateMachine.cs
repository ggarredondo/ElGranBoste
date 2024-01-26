using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    private Transform camTransform;
    private CharacterController characterController;
    private InputController inputController;

    [SerializeField][ReadOnlyField] private string currentStateName;
    private PlayerState currentState;
    [SerializeField] private RunningState runningState;
    [SerializeField] private JokingState jokingState;

    private void Awake() 
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        camTransform = Camera.main.transform;
        characterController = GetComponent<CharacterController>();
        inputController = GetComponent<InputController>();
        inputController.Initialize();

        runningState.Initialize(this);
        jokingState.Initialize(this);
        ChangeState(runningState);
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
        Vector3 forward = Vector3.Cross(camTransform.right, Vector3.up).normalized;
        Vector3 direction = camTransform.right * inputController.MovementDirection.x 
            + forward * inputController.MovementDirection.y;
        transform.LookAt(transform.position + forward);
        characterController.Move(direction * movementSpeed * Time.deltaTime);
    }
    public void Fall() => characterController.Move(Physics.gravity * Time.deltaTime);

    public void TransitionToRunning() => ChangeState(runningState);
    public void TransitionToJoking() => ChangeState(jokingState);

    // Gets
    public ref readonly CharacterController CharacterController => ref characterController;
    public ref readonly InputController InputController => ref inputController;
}
