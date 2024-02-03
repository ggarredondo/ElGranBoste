using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerEffectsBehaviour : MonoBehaviour
{
    private PlayerStateMachine playerStateMachine;

    [Header("Particles")]
    [SerializeField] private ParticleSystem fingerSnapParticles;
    [SerializeField] private float fingerSnapDelay;
    [Space(5)]
    [SerializeField] private ParticleSystem parryParticles;
    [Space(5)]
    [SerializeField] private ParticleSystem deathParticles;

    [Header("Sounds")]
    [SerializeField] private string fingerSnapSoundName;
    [SerializeField] private string deathSoundName;

    private string currentJokeSFX;
    private Sequence snapSequence;

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
        playerStateMachine.JokingState.OnJokePerformed += () =>
        {
            GameManager.Audio.Play(fingerSnapSoundName);

            snapSequence = DOTween.Sequence();
            snapSequence.AppendInterval(fingerSnapDelay);
            snapSequence.AppendCallback(() => fingerSnapParticles.Play());
        };
        playerStateMachine.DeadState.OnEnter += () =>
        {
            deathParticles.Play();
            GameManager.Audio.Play(deathSoundName);
        };

        playerStateMachine.PlayerToEnemyEvents.OnParry += () => parryParticles.Play();

        playerStateMachine.RunningState.OnEnter += () => PauseController.canPauseMenu = true;
        playerStateMachine.RunningState.OnExit += () => PauseController.canPauseMenu = false;
    }
}
