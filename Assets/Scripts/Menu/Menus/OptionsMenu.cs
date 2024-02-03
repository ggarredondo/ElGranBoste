using System.Collections.Generic;
using UnityEngine;

public class OptionsMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private List<UnityEngine.UI.Button> options;
    [SerializeField] private MySlider mouseSensitivity;

    [Header("Transition")]
    [SerializeField] private TransitionPlayer transitionPlayer;
    [SerializeField] private TransitionData customTransitionData;

    protected override void Configure()
    {
        foreach(UnityEngine.UI.Button button in options)
            button.onClick.AddListener(Transition);

        mouseSensitivity.Value = GameManager.Save.Options.mouseSensitivity;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        transitionPlayer.SetCustomData(ref customTransitionData);
        transitionPlayer.startTransitionWithInput.Invoke();
    }

    private void Transition()
    {
        transitionPlayer.endTransitionWithInput.Invoke();
    }

    public void Sensitivity(float value)
    {
        Slider(ref GameManager.Save.Options.mouseSensitivity, value);
    }
}
