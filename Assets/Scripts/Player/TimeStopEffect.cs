using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TimeStopEffect : MonoBehaviour
{
    PlayerStateMachine playerStateMachine;

    [Header("Requirements")]
    [SerializeField] private VolumeProfile volumeProfile;

    [Header("Parameters")]
    [SerializeField] private float delay, stopTime;
    [SerializeField] private List<int> chromaticAberrationList;

    private Sequence sequence;
    private ChromaticAberration chromaticAberration;
    private ColorAdjustments colorAdjustments;

    private void Start()
    {
        if (volumeProfile.TryGet(out ChromaticAberration tmp2)) chromaticAberration = tmp2;
        if (volumeProfile.TryGet(out ColorAdjustments tmp3)) colorAdjustments = tmp3;

        playerStateMachine = GetComponent<PlayerStateMachine>();
        playerStateMachine.PlayerToEnemyEvents.OnParry += ParryEffect;
    }

    private void ParryEffect()
    {
        sequence = DOTween.Sequence();
        sequence.SetUpdate(true);
        sequence.AppendInterval(delay);
        sequence.AppendCallback(() => 
        {
            Time.timeScale = 0f;
            chromaticAberration.intensity.Override(1);
            colorAdjustments.hueShift.Override(chromaticAberrationList[Random.Range(0, chromaticAberrationList.Count)]);
        });
        sequence.AppendInterval(stopTime);
        sequence.AppendCallback(() => 
        {
            Time.timeScale = 1f;
            chromaticAberration.intensity.Override(0);
            colorAdjustments.hueShift.Override(0);
        });
    }
}
