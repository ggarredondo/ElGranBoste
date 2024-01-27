using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MenuController menuController;
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Events")]
    [SerializeField] private EventHandler eventHandler;
    [Space(10)]
    [SerializeField] private List<string> eventNames; 

    [Header("Parameters")]
    [SerializeField] [Range(0f, 1f)] private float slowMotion;

    private bool pauseMenuActivated = false;

    public static System.Action EnterPause;
    public static System.Action ExitPause;

    private void Awake()
    {
        transitionPlayer.Initialize();
        menuController.pauseMenu = true;
    }

    protected void Start()
    {
        menuController.ExitPauseMenuEvent += EnterPauseMenu;

        foreach(string name in eventNames)
            eventHandler.events[name] += EnterPauseMenu;
    }

    protected void OnDestroy()
    {
        menuController.ExitPauseMenuEvent -= EnterPauseMenu;

        foreach (string name in eventNames)
            eventHandler.events[name] -= EnterPauseMenu;

        GameManager.Audio.SetFloat("MusicCutoffFreq", 22000);
    }

    private void EnterPauseMenu()
    {
        if (!pauseMenuActivated) EnterPauseMode();
        else ExitPauseMode();
    }

    public async void EnterPauseMode()
    {
        pauseMenuActivated = true;
        Time.timeScale = slowMotion;

        GameManager.Audio.Play("Pause");
        GameManager.Audio.SetFloat("MusicCutoffFreq", 2000);

        menuController.tree.Initialize();

        EnterPause?.Invoke();

        await transitionPlayer.startTransitionWithInput.Invoke();
    }

    public async void ExitPauseMode()
    {
        pauseMenuActivated = false;
        Time.timeScale = 1;

        GameManager.Audio.Play("Pause");
        GameManager.Audio.SetFloat("MusicCutoffFreq", 22000);

        menuController.DisableMenus();

        transitionPlayer.SetDefaultData();
        await transitionPlayer.endTransitionWithInput.Invoke();

        ExitPause?.Invoke();
    }
}
