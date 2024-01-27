using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.InputSystem;
using DG.Tweening;

public class JokeBook : MonoBehaviour
{
    private const string FOLDER = "Pages/Default";

    [Header("Requirements")]
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private GameObject parent;

    [Header("Parameters")]
    [SerializeField] private Vector3 selectedPositionOffset;
    [SerializeField] private float selectedAnimationTime;

    private List<BookPage> pages;
    private bool selected;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
        player.InputController.OnMouseWheel += MouseWheel;
        pages = new();
        Initialize();
    }

    private void OnDestroy()
    {
        player.InputController.OnMouseWheel -= MouseWheel;
    }

    private void MouseWheel(float direction)
    {
        if (selected)
            MovePages(direction);
        else
        {
            transform.DOLocalMove(initialPosition + selectedPositionOffset, 1);
            selected = true;
        }
    }

    private async void MovePages(float direction)
    {
        if (direction == -120 && player.SelectedJoke < pages.Count)
        {
            if (player.SelectedJoke + 1 < pages.Count)
                pages[player.SelectedJoke + 1].gameObject.SetActive(true);

            pages[player.SelectedJoke].MoveForward();
            player.SetSelectedJoke(player.SelectedJoke + 1);
        }

        if (direction == 120 && player.SelectedJoke > 0)
        {
            player.SetSelectedJoke(player.SelectedJoke - 1);

            await pages[player.SelectedJoke].MoveBackWards();

            if (player.SelectedJoke + 1 < pages.Count)
                pages[player.SelectedJoke + 1].gameObject.SetActive(false);
        }
    }
    
    private void Initialize()
    {
        int tmp = 0;

        foreach(Joke joke in player.JokeList)
        {
            GameObject page = LoadPage("Joke_" + tmp, joke.Sentence);
            page.SetActive(false);
            pages.Add(page.GetComponent<BookPage>());
            pages[tmp].SetStyle(joke.Sentence);
            tmp++;
        }

        pages[0].gameObject.SetActive(true);
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

    public GameObject LoadPage(string name, string joke)
    {
        string path = FOLDER;

        GameObject currentCard = Instantiate((GameObject)LoadPrefabStyleFromFile(path), parent.transform);

        currentCard.name = name;

        return currentCard;
    }
}
