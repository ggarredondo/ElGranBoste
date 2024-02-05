using System.Threading.Tasks;
using UnityEngine;
using LerpUtilities;
using System.Threading;
using DG.Tweening;

public class TransitionPlayer : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private RectTransform leftTransform;
    [SerializeField] private RectTransform rightTransform;
    [SerializeField] private TransitionData defaultData;

    [Header("Parameters")]
    [SerializeField] private float lerpDuration;

    public System.Func<Task> startTransition;
    public System.Func<Task> endTransition;
    public System.Func<Task> startTransitionWithInput;
    public System.Func<Task> endTransitionWithInput;

    private CancellationTokenSource cancellationTokenSource;
    private TransitionData data;

    public Sequence sequence;

    public void Initialize()
    {
        data = new(defaultData);
        cancellationTokenSource = new();

        startTransition += StartTransitionPlayer;
        endTransition += EndTransitionPlayer;

        startTransitionWithInput += StartTransitionPlayerWithInput;
        endTransitionWithInput += EndTransitionPlayerWithInput;

        sequence = DOTween.Sequence();
    }

    public void SetCamera(Camera camera)
    {
        GetComponent<Canvas>().worldCamera = camera;
    }

    public void SetCustomData(ref TransitionData data)
    {
        this.data = data;
    }

    public void SetDefaultData()
    {
        data = defaultData;
    }

    private void OnDestroy()
    {
        startTransition -= StartTransitionPlayer;
        endTransition -= EndTransitionPlayer;

        startTransitionWithInput -= StartTransitionPlayerWithInput;
        endTransitionWithInput -= EndTransitionPlayerWithInput;
    }

    private void ResetCancelToken()
    {
        cancellationTokenSource.Cancel();
        cancellationTokenSource.Dispose();
        cancellationTokenSource = new();
    }

    private async Task StartTransitionPlayerWithInput()
    {
        ResetCancelToken();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        try
        {
            await Task.WhenAll(Lerp.Value_Unscaled(leftTransform.anchoredPosition, data.startLeftPosition, (a) => leftTransform.anchoredPosition = a, lerpDuration, cancellationToken),
                               Lerp.Value_Unscaled(rightTransform.anchoredPosition, data.startRightPosition, (a) => rightTransform.anchoredPosition = a, lerpDuration, cancellationToken));
        }
        catch (TaskCanceledException) { return; }

        GameManager.Audio.Play(data.startSound);
    }

    private async Task EndTransitionPlayerWithInput()
    {
        ResetCancelToken();
        CancellationToken cancellationToken = cancellationTokenSource.Token;

        GameManager.Audio.Play(data.endSound);

        try
        {
            await Task.WhenAll(Lerp.Value_Unscaled(leftTransform.anchoredPosition, data.endLeftPosition, (a) => leftTransform.anchoredPosition = a, lerpDuration, cancellationToken),
                               Lerp.Value_Unscaled(rightTransform.anchoredPosition, data.endRightPosition, (a) => rightTransform.anchoredPosition = a, lerpDuration, cancellationToken));
        }
        catch (TaskCanceledException) { return; }
    }

    private async Task StartTransitionPlayer()
    {
        GameManager.Input.ActivatePlayerInput(false);
        GameManager.Input.EnableUIModule(false);

        await Task.WhenAll(Lerp.Value(leftTransform.anchoredPosition, data.startLeftPosition, (a) => leftTransform.anchoredPosition = a, lerpDuration),
                           Lerp.Value(rightTransform.anchoredPosition, data.startRightPosition, (a) => rightTransform.anchoredPosition = a, lerpDuration));

        GameManager.Audio.Play(data.startSound);
    }

    private async Task EndTransitionPlayer()
    {
        GameManager.Audio.Play(data.endSound);

        await Task.WhenAll(Lerp.Value(data.startLeftPosition, data.endLeftPosition, (a) => leftTransform.anchoredPosition = a, lerpDuration),
                           Lerp.Value(data.startRightPosition, data.endRightPosition, (a) => rightTransform.anchoredPosition = a, lerpDuration));

        GameManager.Input.EnableUIModule(true);
    }
}
