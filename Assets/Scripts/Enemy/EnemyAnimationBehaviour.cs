using UnityEngine;

public class EnemyAnimationBehaviour : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private EnemyStateMachine enemyStateMachine;

    private void Start()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();
    }
}
