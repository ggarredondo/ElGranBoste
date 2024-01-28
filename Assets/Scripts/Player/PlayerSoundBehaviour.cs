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

    private string currentJokeSFX;

    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();

        playerStateMachine.JokingState.OnEnter += () =>
        {
            currentJokeSFX = playerStateMachine.JokeList[playerStateMachine.SelectedJoke].SFX;
            GameManager.Audio.Play(currentJokeSFX);
        };
        playerStateMachine.JokingState.OnExit += () =>
        {
            GameManager.Audio.Stop(currentJokeSFX);
        };
        playerStateMachine.JokingState.OnJokePerformed += () => GameManager.Audio.Play(fingerSnapSoundName);
        playerStateMachine.DeadState.OnEnter += () => GameManager.Audio.Play(deathSoundName);
    }
}
