using DG.Tweening;
using UnityEngine;

public class TimeStopEffect : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;
    [SerializeField] private float delay, stopTime;
    private Sequence sequence;
    private void Start()
    {
        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerStateMachine.PlayerToEnemyEvents.OnParry += ParryEffect;
    }

    private void ParryEffect()
    {
        sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        sequence.AppendInterval(delay);
        sequence.AppendCallback(() => Time.timeScale = 0f);
        sequence.AppendInterval(stopTime);
        sequence.AppendCallback(() => Time.timeScale = 1f);
    }
}
