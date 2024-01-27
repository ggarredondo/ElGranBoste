using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class JokeBook : MonoBehaviour
{
    private const string FOLDER = "Pages/Default";

    [Header("Requirements")]
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private GameObject parent;

    [Header("Input")]

    private List<Transform> pages;

    private void Start()
    {
        pages = new();
        Initialize();
    }

    private void Initialize()
    {
        int tmp = 0;

        foreach(Joke joke in player.JokeList)
        {
            GameObject page = LoadPage("Joke_" + tmp, joke.Sentence);
            pages.Add(page.transform);
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
