using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Rendering;

public class PauseController : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private MenuController menuController;
    [SerializeField] private TransitionPlayer transitionPlayer;
    [SerializeField] private InputReader inputReader;
    [SerializeField] private GameObject hud;

    [Header("PostProcessing")]
    [SerializeField] private Volume volume;
    [SerializeField] private VolumeProfile pausedVolumeProfile;
    [SerializeField] private VolumeProfile unpausedVolumeProfile;

    [Header("Parameters")]
    [SerializeField] [Range(0f, 1f)] private float slowMotion;

    public static bool pauseMenuActivated = false;
    public static bool canPauseMenu = true;

    public static System.Action EnterPause;
    public static System.Action ExitPause;

    private void Awake()
    {
        transitionPlayer.Initialize();
        menuController.pauseMenu = true;
    }

    protected void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        menuController.ExitPauseMenuEvent += EnterPauseMenu;
        inputReader.StartPauseMenuEvent += EnterPauseMenu;
    }

    protected void OnDestroy()
    {
        menuController.ExitPauseMenuEvent -= EnterPauseMenu;
        inputReader.StartPauseMenuEvent -= EnterPauseMenu;

        GameManager.Audio.SetFloat("MusicCutoffFreq", 22000);
    }

    private void EnterPauseMenu()
    {
        if (!pauseMenuActivated && canPauseMenu) EnterPauseMode();
        else if(canPauseMenu) ExitPauseMode();
    }

    private void Update()
    {
        if(!pauseMenuActivated)
            Cursor.visible = false;

    }

    public async void EnterPauseMode()
    {
        DOTween.PauseAll();
        pauseMenuActivated = true;

        Time.timeScale = slowMotion;

        hud.SetActive(false);
        volume.profile = pausedVolumeProfile;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GameManager.Input.EnterPauseMenu();
        GameManager.Audio.Play("Pause");
        GameManager.Audio.SetFloat("MusicCutoffFreq", 2000);
        GameManager.Audio.PauseAllSounds("Sfx");

        menuController.tree.Initialize();

        EnterPause?.Invoke();

        await transitionPlayer.startTransitionWithInput.Invoke();
    }

    public async void ExitPauseMode()
    {
        pauseMenuActivated = false;
        Time.timeScale = 1;

        hud.SetActive(true);
        volume.profile = unpausedVolumeProfile;
        Cursor.lockState = CursorLockMode.Locked;

        GameManager.Input.ExitPauseMenu();
        GameManager.Audio.Play("Pause");
        GameManager.Audio.SetFloat("MusicCutoffFreq", 22000);
        GameManager.Audio.ResumeAllSounds("Sfx");

        menuController.DisableMenus();

        transitionPlayer.SetDefaultData();

        foreach (Tween tween in DOTween.PausedTweens())
        {
            tween.Play();
        }

        await transitionPlayer.endTransitionWithInput.Invoke();

        ExitPause?.Invoke();
    }
}
