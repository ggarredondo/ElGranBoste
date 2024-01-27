using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEventBehaviour : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [SerializeField] private UnityEvent OnEnterRunningState;
    [SerializeField] private UnityEvent OnExitRunningState;
    [SerializeField] private UnityEvent OnEnterJokingState;
    [SerializeField] private UnityEvent OnExitJokingState;
    [SerializeField] private UnityEvent OnJokePerformedJokingState;
    [SerializeField] private UnityEvent OnEnterDeathState;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.RunningState.OnEnter += () => OnEnterRunningState.Invoke();
        playerStateMachine.RunningState.OnExit += () => OnExitRunningState.Invoke();

        playerStateMachine.JokingState.OnEnter += () => OnEnterJokingState.Invoke();
        playerStateMachine.JokingState.OnExit += () => OnExitJokingState.Invoke();
        playerStateMachine.JokingState.OnJokePerformed += () => OnJokePerformedJokingState.Invoke();

        playerStateMachine.DeadState.OnEnter += () => OnEnterDeathState.Invoke();
    }
}
