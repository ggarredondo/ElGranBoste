using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;
using System.Threading;

public class BookPage : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private TMP_Text jokeText;
    [SerializeField] private TMP_Text timeText;

    [Header("Parameters")]
    [SerializeField] float pageRotationAngle;
    [SerializeField] float initialPageRotationAngle;
    [SerializeField] float pageRotationTime;

    [Header("Sounds")]
    [SerializeField] private string movePageSoundName;

    private Sequence sequence;

    public void SetStyle(string joke, JokeType jokeType, int time)
    {
        jokeText.text = joke;
        jokeText.color = jokeType == JokeType.Poste ? Color.red : Color.black;
        timeText.text = time.ToString() + " seg.";
    }

    public void MoveForward()
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, pageRotationAngle), pageRotationTime);
    }

    public void MoveBackWardsAsync(BookPage nextPage)
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime);

        sequence = DOTween.Sequence();
        sequence.AppendInterval(pageRotationTime - 0.1f);
        sequence.OnComplete(() => nextPage.DisablePage());
    }

    public void MoveBackWards()
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime);
    }

    public void CancelDisable(BookPage nextPage)
    {
        sequence.Kill();
        nextPage.gameObject.SetActive(true);
    }

    public void DisablePage()
    {
        gameObject.SetActive(false);
    }

    public async void DestroyPage()
    {
        await transform.DOLocalMove(new Vector3(-1,1,0), pageRotationTime).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
