using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySoundBehaviour : MonoBehaviour
{
    private EnemyStateMachine enemyStateMachine;

    [Header("Sounds")]
    [SerializeField] private float walkingSoundPitch;
    [SerializeField] private float listeningSoundPitch;

    [Header("Sounds")]
    [SerializeField] private string laughSoundName;
    [SerializeField] private string walkingSoundName;
    [SerializeField] private string listeningSoundName;
    [SerializeField] private string posteSoundName;

    private void Start()
    {
        enemyStateMachine = GetComponent<EnemyStateMachine>();

        enemyStateMachine.ChasingState.OnEnter += () => GameManager.Audio.Play(walkingSoundName);
        enemyStateMachine.ChasingState.OnExit += () => GameManager.Audio.Stop(walkingSoundName);

        enemyStateMachine.LaughingState.OnEnter += () => GameManager.Audio.Play(laughSoundName);
        enemyStateMachine.LaughingState.OnExit += () => GameManager.Audio.Stop(laughSoundName);

        enemyStateMachine.ListeningState.OnEnter += () => 
        { 
            GameManager.Audio.Play(listeningSoundName);
            GameManager.Audio.ChangePitch(walkingSoundName, listeningSoundPitch);
            GameManager.Audio.Play(walkingSoundName);
        };

        enemyStateMachine.ListeningState.OnExit += () =>
        {
            GameManager.Audio.Stop(listeningSoundName);
            GameManager.Audio.ChangePitch(walkingSoundName, walkingSoundPitch);
            GameManager.Audio.Stop(walkingSoundName);
        };

        enemyStateMachine.PosteState.OnEnter += () => GameManager.Audio.Play(posteSoundName);
    }
}
