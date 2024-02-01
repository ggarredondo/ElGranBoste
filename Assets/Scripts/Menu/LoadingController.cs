using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneUtilities;
using DG.Tweening;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private float loadingTime;

    private bool isLoaded;
    private Sequence sequence;

    private void Start()
    {
        SceneLoader.UpdateLoading += UpdateLoading;

        Timer();
    }

    private void OnDestroy()
    {
        SceneLoader.UpdateLoading -= UpdateLoading;
    }

    private void Timer()
    {
        sequence = DOTween.Sequence();
        sequence.AppendInterval(loadingTime);
        sequence.OnComplete(() => isLoaded = true);
    }

    private bool UpdateLoading(float progress)
    {
        return isLoaded;
    }
}
