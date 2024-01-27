using UnityEngine;

public class EnemyAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private EnemyStateMachine enemyStateMachine;

    private void Start()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();

        enemyStateMachine.ChasingState.OnEnter += () => animator.SetBool("STATE_CHASING", true);
        enemyStateMachine.ChasingState.OnExit += () => animator.SetBool("STATE_CHASING", false);

        enemyStateMachine.ListeningState.OnEnter += () => animator.SetBool("STATE_LISTENING", true);
        enemyStateMachine.ListeningState.OnExit += () => animator.SetBool("STATE_LISTENING", false);

        enemyStateMachine.LaughingState.OnEnter += () => animator.SetTrigger("laugh");

        enemyStateMachine.PosteState.OnEnter += () => animator.SetBool("STATE_POSTE", true);
        enemyStateMachine.PosteState.OnExit += () => animator.SetBool("STATE_POSTE", false);
    }
}
