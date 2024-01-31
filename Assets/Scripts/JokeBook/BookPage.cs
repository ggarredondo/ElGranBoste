using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

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

    public async Task MoveBackWardsAsync()
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime);

        await Task.Delay(System.TimeSpan.FromSeconds(pageRotationTime - 0.1f));
    }

    public void MoveBackWards()
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime);
    }

    public async void DisablePage(Task task)
    {
        await task;
        gameObject.SetActive(false);
    }

    public async void DestroyPage()
    {
        await transform.DOLocalMove(new Vector3(-1,1,0), pageRotationTime).AsyncWaitForCompletion();
        Destroy(gameObject);
    }
}
