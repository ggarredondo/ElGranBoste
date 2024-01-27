using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private UnityEngine.UI.Button resumeButton;
    [SerializeField] private UnityEngine.UI.Button exitButton;

    [Header("Transition")]
    [SerializeField] private TransitionPlayer transitionPlayer;

    [Header("Sounds")]
    [SerializeField] private string playSoundName;

    protected override void Configure()
    {
        resumeButton.onClick.AddListener(PlayGame);
        exitButton.onClick.AddListener(Exit);
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        transitionPlayer.SetDefaultData();
    }

    private void PlayGame()
    {
        GameManager.Audio.Play(playSoundName);
        GameManager.Scene.NextScene();
    }

    private void Exit()
    {
        Application.Quit();
    }
}
