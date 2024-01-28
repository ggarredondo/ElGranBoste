using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class JokeBook : MonoBehaviour
{
    private const string FOLDER = "Pages/Default";

    [Header("Requirements")]
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private GameObject parent;

    [Header("Parameters")]
    [SerializeField] private Vector3 selectedPosition;
    [SerializeField] private Vector3 unselectedPosition;
    [SerializeField] private float selectedAnimationTime;
    [SerializeField] private float unselectedAnimationTime;

    private List<BookPage> pages;
    private bool selected, setTimer;
    private Sequence timer;

    private void Start()
    {
        player.InputController.OnMouseWheel += MouseWheel;
        player.OnPickBook += UpdateBookPages;
        player.PlayerToEnemyEvents.OnJokePerformed += RemoveJoke;
        pages = new();
        Initialize();
    }

    private void RemoveJoke(in Joke joke)
    {
        if (player.SelectedJoke == pages.Count - 1 && player.SelectedJoke > 0)
        {
            pages[player.SelectedJoke - 1].MoveBackWards();
            player.SetSelectedJoke(player.SelectedJoke - 1);
        }

        int index = player.JokeList.IndexOf(joke);

        pages[index].DestroyPage();
        pages.RemoveAt(index);
        player.JokeList.RemoveAt(index);

        if(pages.Count > 0)
            pages[player.SelectedJoke].gameObject.SetActive(true);
    }

    private void UpdateBookPages()
    {
        for(int i = pages.Count; i < player.JokeList.Count; i++)
        {
            GameObject page = LoadPage("Joke_" + i);
            page.SetActive(false);
            pages.Add(page.GetComponent<BookPage>());
            pages[i].SetStyle(player.JokeList[i].ID, (int)player.JokeList[i].TimeToPerform);
        }

        pages[0].gameObject.SetActive(true);
    }

    private void Initialize()
    {
        int tmp = 0;

        foreach (Joke joke in player.JokeList)
        {
            GameObject page = LoadPage("Joke_" + tmp);
            page.SetActive(false);
            pages.Add(page.GetComponent<BookPage>());
            pages[tmp].SetStyle(joke.ID, (int) joke.TimeToPerform);
            tmp++;
        }

        pages[0].gameObject.SetActive(true);
    }

    private void OnDestroy()
    {
        player.InputController.OnMouseWheel -= MouseWheel;
        player.OnPickBook -= UpdateBookPages;
        player.PlayerToEnemyEvents.OnJokePerformed -= RemoveJoke;
    }

    private void MouseWheel(float direction)
    {
        if (selected)
        {
            MovePages(direction);
            BookTimer();
        }
        else
        {
            transform.DOLocalMove(selectedPosition, selectedAnimationTime);
            selected = true;
        }
    }

    private async void BookTimer()
    {
        if (setTimer)
        {
            timer.Kill();
        }

        setTimer = true;
        timer = DOTween.Sequence();
        timer.AppendInterval(unselectedAnimationTime);
        timer.Append(transform.DOLocalMove(unselectedPosition, selectedAnimationTime));
        await timer.AsyncWaitForCompletion();
        selected = false;
    }

    private async void MovePages(float direction)
    {
        if (direction == -120 && player.SelectedJoke < pages.Count-1)
        {
            if (player.SelectedJoke + 1 < pages.Count)
                pages[player.SelectedJoke + 1].gameObject.SetActive(true);

            pages[player.SelectedJoke].MoveForward();
            player.SetSelectedJoke(player.SelectedJoke + 1);
        }

        if (direction == 120 && player.SelectedJoke > 0)
        {
            player.SetSelectedJoke(player.SelectedJoke - 1);

            await pages[player.SelectedJoke].MoveBackWardsAsync();

            if (player.SelectedJoke + 1 < pages.Count)
                pages[player.SelectedJoke + 1].gameObject.SetActive(false);
        }
    }

    private Object LoadPrefabStyleFromFile(string path)
    {
        var loadedObject = Resources.Load(path);
        if (loadedObject == null)
        {
            Debug.LogWarning("...no file found - please check the configuration");
        }
        return loadedObject;
    }

    public GameObject LoadPage(string name)
    {
        string path = FOLDER;

        GameObject currentCard = Instantiate((GameObject)LoadPrefabStyleFromFile(path), parent.transform);

        currentCard.name = name;

        return currentCard;
    }
}
