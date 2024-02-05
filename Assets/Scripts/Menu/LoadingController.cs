using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SceneUtilities;
using UnityEngine.InputSystem;
using TMPro;
using DG.Tweening;

public class LoadingController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private GameObject continueObject;
    [SerializeField] private TMP_Text loadText;
    [SerializeField] private InputActionReference actionReference;

    [Header("Parameters")]
    [SerializeField] private string continueText;
    [Space(5)]
    [SerializeField] private float endScaleAnimation;
    [SerializeField] private float animationDuration;

    private bool isLoaded;
    private Sequence continueSequence;
    private RectTransform rectTransform;

    private void Start()
    {
        SceneLoader.UpdateLoading += UpdateLoading;
        actionReference.action.performed += TriggerAction;

        rectTransform = continueObject.GetComponent<RectTransform>();
        loadText.text = continueText;
        continueObject.SetActive(false);
        InitializeAnimation();
    }

    private void OnDestroy()
    {
        SceneLoader.UpdateLoading -= UpdateLoading;
        actionReference.action.performed -= TriggerAction;

        continueSequence.Kill();
    }

    private void TriggerAction(InputAction.CallbackContext ctx)
    {
        isLoaded = true;
    }

    private void InitializeAnimation()
    {
        continueSequence = DOTween.Sequence();
        continueSequence.Append(rectTransform.DOScale(endScaleAnimation, animationDuration));
        continueSequence.SetLoops(-1, LoopType.Yoyo);
    }

    private bool UpdateLoading(float progress)
    {
        if (progress >= 0.9f)
        {
            continueObject.SetActive(true);
            return isLoaded;
        }

        isLoaded = false;
        return isLoaded;
    }
}
