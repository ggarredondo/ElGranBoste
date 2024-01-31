using DG.Tweening;
using UnityEngine;

public class ParryAnimation : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine playerStateMachine;

    private Sequence parrySequence;

    [Header("Sounds")]
    [SerializeField] private string wooshSoundName;
    [SerializeField] private string parrySoundName;

    [Header("Parameters")]
    [SerializeField] private Vector3 startUpPosition;
    [SerializeField] private Vector3 activePosition;
    [SerializeField] private Quaternion activeRotation;
    [SerializeField] private Vector3 recoveryPosition;
    [SerializeField] private Quaternion recoveryRotation;
    [SerializeField] private Ease activeEase;
    [SerializeField] private Ease recoveryEase;

    private void Start()
    {
        playerStateMachine.ParryState.OnEnter += Parry;
        playerStateMachine.PlayerToEnemyEvents.OnParry += PlaySound;
        playerStateMachine.DeadState.OnEnter += () => GameManager.Audio.Stop(parrySoundName);
    }

    private void PlaySound()
    {
        GameManager.Audio.Play(parrySoundName);
    }

    private void PlayWooshSound()
    {
        GameManager.Audio.Play(wooshSoundName);
    }

    private void Parry()
    {
        parrySequence = DOTween.Sequence();
        parrySequence.Append(transform.DOLocalMove(startUpPosition, playerStateMachine.ParryState.StartUp));
        parrySequence.AppendCallback(PlayWooshSound);
        parrySequence.Append(transform.DOLocalMove(activePosition, playerStateMachine.ParryState.Active).SetEase(activeEase));
        parrySequence.Join(transform.DOLocalRotateQuaternion(activeRotation, playerStateMachine.ParryState.Active).SetEase(activeEase));
        parrySequence.Append(transform.DOLocalMove(recoveryPosition, playerStateMachine.ParryState.Recovery).SetEase(recoveryEase));
        parrySequence.Join(transform.DOLocalRotateQuaternion(recoveryRotation, playerStateMachine.ParryState.Recovery).SetEase(recoveryEase));
    }
}
