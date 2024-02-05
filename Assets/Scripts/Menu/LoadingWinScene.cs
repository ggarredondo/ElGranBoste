using UnityEngine;
using DG.Tweening;
using TMPro;

public class LoadingWinScene : LoadingController
{
    [Header("Winner Animation")]
    [SerializeField] private RectTransform titleTransform;
    [SerializeField] private RectTransform lastScoreTransform;
    [Space(5)]
    [SerializeField] private TMP_Text lastScoreText;
    [Space(5)]
    [SerializeField] ParticleSystem particles;

    [Header("Sounds")]
    [SerializeField] private string exploteSound;
    [SerializeField] private string timeSound;

    private Sequence titleSequence;
    private Sequence scoreSequence;
    private Sequence particlesSequence;

    protected override void Start()
    {
        base.Start();

        SetTime(ref lastScoreText, GameManager.Save.Game.lastScore);

        GameManager.Audio.Play("WinnerAmbient");
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        titleSequence.Kill();
        scoreSequence.Kill();
        particlesSequence.Kill();
    }

    protected override void InitializeAnimation()
    {
        base.InitializeAnimation();

        particlesSequence = DOTween.Sequence();
        particlesSequence.AppendInterval(0.5f);
        particlesSequence.AppendCallback(() => { particles.Play(); GameManager.Audio.Play(exploteSound); });
        particlesSequence.AppendInterval(1f);
        particlesSequence.AppendCallback(() => GameManager.Audio.Play(timeSound));

        titleSequence = DOTween.Sequence();
        titleSequence.AppendInterval(0.5f);
        titleSequence.Append(titleTransform.DOScale(1, 1).SetEase(Ease.OutBounce));
        titleSequence.Append(lastScoreTransform.DOScale(1, 1).SetEase(Ease.OutBounce));

        scoreSequence = DOTween.Sequence();
        scoreSequence.Append(lastScoreTransform.DOShakeAnchorPos(1, strength: 10, vibrato: 10, fadeOut: false, randomnessMode: ShakeRandomnessMode.Full));
        scoreSequence.SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
    }

    private void SetTime(ref TMP_Text tmpText, float elapsedTime)
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        int milliseconds = Mathf.FloorToInt((elapsedTime * 1000) % 1000);

        if(seconds < 10)
            tmpText.text = minutes.ToString() + ":0" + seconds.ToString() + ":" + milliseconds.ToString();
        else
            tmpText.text = minutes.ToString() + ":" + seconds.ToString() + ":" + milliseconds.ToString();
    }
}
