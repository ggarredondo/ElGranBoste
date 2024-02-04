using UnityEngine;

public class PlayerAnimatorBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerStateMachine playerStateMachine;

    private void Awake()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.RunningState.OnEnter += () => animator.SetBool("STATE_RUNNING", true);
        playerStateMachine.RunningState.OnExit += () => animator.SetBool("STATE_RUNNING", false);

        playerStateMachine.JokingState.OnEnter += () => animator.SetBool("STATE_JOKING", true);
        playerStateMachine.JokingState.OnExit += () => animator.SetBool("STATE_JOKING", false);
        playerStateMachine.JokingState.OnJokePerformed += () => animator.SetTrigger("jokePerformed");

        playerStateMachine.ParryState.OnEnter += () => animator.SetBool("STATE_PARRY", true);
        playerStateMachine.ParryState.OnExit += () => animator.SetBool("STATE_PARRY", false);

        playerStateMachine.DeadState.OnEnter += () => animator.SetBool("STATE_DEAD", true);
    }

    private void Update()
    {
        animator.SetBool("is_moving", playerStateMachine.Velocity.magnitude > 0f);
    }
}
