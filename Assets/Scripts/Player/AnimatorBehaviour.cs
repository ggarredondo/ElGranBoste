using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private PlayerStateMachine playerStateMachine;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.RunningState.OnEnter += () => animator.SetBool("STATE_RUNNING", true);
        playerStateMachine.RunningState.OnExit += () => animator.SetBool("STATE_RUNNING", false);

        playerStateMachine.JokingState.OnEnter += () => animator.SetBool("STATE_JOKING", true);
        playerStateMachine.JokingState.OnExit += () => animator.SetBool("STATE_JOKING", false);
        playerStateMachine.JokingState.OnJokePerformed += () => animator.SetTrigger("jokePerformed");
    }

    private void Update()
    {
        animator.SetBool("is_moving", playerStateMachine.Velocity.magnitude > 0f);
    }
}
