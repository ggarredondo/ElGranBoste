using UnityEngine;

public class AnimatorBehaviour : MonoBehaviour
{
    private Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        PlayerStateMachine stateMachine = GetComponent<PlayerStateMachine>();
    }
}
