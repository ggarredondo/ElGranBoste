using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventBehaviour : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private UnityEvent OnEnterParryState;
    [SerializeField] private UnityEvent OnEnterDeathState;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.PlayerToEnemyEvents.OnParry += () => OnEnterParryState.Invoke();
        playerStateMachine.DeadState.OnEnter += () => OnEnterDeathState.Invoke();
    }
}
