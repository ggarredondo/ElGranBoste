using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using UnityEngine.InputSystem;

public class JokeBook : MonoBehaviour
{
    private const string FOLDER = "Pages/Default";

    [Header("Requirements")]
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private GameObject parent;

    private List<BookPage> pages;

    private void Start()
    {
        player.InputController.OnMouseWheel += MovePages;
        pages = new();
        Initialize();
    }

    private void OnDestroy()
    {
        player.InputController.OnMouseWheel -= MovePages;
    }

    private void MovePages(float direction)
    {
        if(direction == -120 && player.SelectedJoke < pages.Count)
        {
            pages[player.SelectedJoke].MoveForward();

            if(player.SelectedJoke + 1 < pages.Count)
                player.SetSelectedJoke(player.SelectedJoke + 1);
        }
        
        if(direction == 120 && player.SelectedJoke > 0)
        {
            pages[player.SelectedJoke].MoveBackWards();
            player.SetSelectedJoke(player.SelectedJoke - 1);
        }
    }
    
    private void Initialize()
    {
        int tmp = 0;

        foreach(Joke joke in player.JokeList)
        {
            GameObject page = LoadPage("Joke_" + tmp, joke.Sentence);
            pages.Add(page.GetComponent<BookPage>());
            pages[tmp].SetStyle(joke.Sentence);
            tmp++;
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

    public GameObject LoadPage(string name, string joke)
    {
        string path = FOLDER;

        GameObject currentCard = Instantiate((GameObject)LoadPrefabStyleFromFile(path), parent.transform);

        currentCard.name = name;

        return currentCard;
    }
}
