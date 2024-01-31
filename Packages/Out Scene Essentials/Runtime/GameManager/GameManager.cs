using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

using InputUtilities;
using SaveUtilities;
using SceneUtilities;
using AudioUtilities;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private bool mainMenu;

    [Header("EventSettings")]
    [SerializeField] private List<EventHandler> eventHandler;

    [Header("SaveSettings")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private SaveOptions defaultOptions;
    [SerializeField] private SaveGame defaultGame;

    [Header("SceneSettings")]
    [SerializeField] private List<SceneLogic> scenes;
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Debug")]
    [SerializeField] [Range(0f, 1f)] private float timeScale = 1f;

    private static ISave saver;
    private static IChangeScene sceneController;
    private static InputFacade inputUtilities;
    private static AudioController audioController;

    public static ref readonly ISave Save { get => ref saver; }
    public static ref readonly IChangeScene Scene { get => ref sceneController; }
    public static ref readonly InputFacade Input { get => ref inputUtilities; }
    public static ref readonly AudioController Audio { get => ref audioController; }

    public static int RANDOM_SEED => System.Guid.NewGuid().GetHashCode();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        //Collision Initialize
        eventHandler.ForEach(c => c.Initialize());

        //Audio Initialize
        audioController = new(ref audioMixer);

        //Scene Initialize
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        sceneController = new SceneController(SceneManager.GetActiveScene().name, scenes, ref transitionPlayer);
        transitionPlayer.Initialize();

        //Save Initialize
        saver = new DataSaver(defaultOptions, defaultGame, audioMixer);
        saver.Load();

        //Input Initialize
        inputUtilities = new(GetComponent<InputSystemUIInputModule>(), mainMenu);
    }

    private void Start()
    {
        saver.ApplyChanges();
    }

    private void OnDestroy()
    {
        saver.Save();
    }

    private void Update()
    {
        if(mainMenu)
            inputUtilities.Update();
    }

    private void OnValidate()
    {
        Time.timeScale = timeScale;
    }

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        inputUtilities.OnPlayerJoined(playerInput);

    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1;

        if (scene.name != sceneController.GetCurrentLoadScene())
        {
            SceneLogic currentSceneLogic = sceneController.GetSceneLogic(scene.name);

            if (currentSceneLogic.gameObjectsTag.Count > 0)
            {
                foreach (string tag in currentSceneLogic.gameObjectsTag)
                    GameObject.FindGameObjectWithTag(tag).GetComponent<IObjectInitialize>().Initialize();
            }

            AudioController.InitializeSound?.Invoke();

            transitionPlayer.SetCamera(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>());

            if (currentSceneLogic.playMusic)
                audioController.Play(currentSceneLogic.music);
        }
    }

    void OnSceneUnloaded(Scene scene)
    {
        AudioController.InitializeSound = null;
    }
}
