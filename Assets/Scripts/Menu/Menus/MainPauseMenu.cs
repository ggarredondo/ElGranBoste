using UnityEngine;

public class MainPauseMenu : AbstractMenu
{
    [SerializeField] private PauseController pauseController;

    [Header("UI Elements")]
    [SerializeField] private UnityEngine.UI.Button resumeButton;
    [SerializeField] private UnityEngine.UI.Button backButton;
    [SerializeField] private UnityEngine.UI.Button resetButton;

    [Header("Transition")]
    [SerializeField] private TransitionPlayer transitionPlayer;

    protected override void Configure()
    {
        resumeButton.onClick.AddListener(ResumeGame);
        backButton.onClick.AddListener(Exit);
        resetButton.onClick.AddListener(ResetPlayers);
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

    private void ResetPlayers()
    {
        Time.timeScale = 1;
        GameManager.Scene.PreviousScene(transition : false);
    }

    private void Exit()
    {
        Application.Quit();
    }
}
