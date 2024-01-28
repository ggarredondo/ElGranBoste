using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSoundBehaviour : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [Header("Sounds")]
    [SerializeField] private string fingerSnapSoundName;
    [SerializeField] private string deathSoundName;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.JokingState.OnEnter += () => GameManager.Audio.Play(playerStateMachine.JokeList[playerStateMachine.SelectedJoke].SFX);
        playerStateMachine.JokingState.OnExit += () => GameManager.Audio.Stop(playerStateMachine.JokeList[playerStateMachine.SelectedJoke].SFX);
        playerStateMachine.JokingState.OnJokePerformed += () => GameManager.Audio.Play(fingerSnapSoundName);
        playerStateMachine.DeadState.OnEnter += () => GameManager.Audio.Play(deathSoundName);
    }
}
