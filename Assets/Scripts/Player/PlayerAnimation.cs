using UnityEngine;

[System.Serializable]
public class PlayerAnimation
{
    private Animator animator;
    public void Initialize(in PlayerStateMachine stateMachine, in Animator animator)
    {
        this.animator = animator;
    }
}
