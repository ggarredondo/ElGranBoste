using UnityEngine;
using DG.Tweening;

public class MainPauseMenu : AbstractMenu
{
    [SerializeField] private PauseController pauseController;

    [Header("UI Elements")]
    [SerializeField] private UnityEngine.UI.Button resumeButton;
    [SerializeField] private UnityEngine.UI.Button backButton;

    [Header("Transition")]
    [SerializeField] private TransitionPlayer transitionPlayer;

    protected override void Configure()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        backButton.onClick.AddListener(Exit);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        transitionPlayer.SetDefaultData();
    }

    private void ResumeGame()
    {
        pauseController.ExitPauseMode();
    }

    private void Exit()
    {
        Time.timeScale = 1;
        PauseController.pauseMenuActivated = false;
        DOTween.CompleteAll();
        GameManager.Scene.NextScene(transition : false);
    }
}
