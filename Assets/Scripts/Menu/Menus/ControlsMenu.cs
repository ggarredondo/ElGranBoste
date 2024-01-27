using InputUtilities;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : AbstractMenu
{ 
    [Header("UI Elements")]
    [SerializeField] private MyToggle rumble;
    [SerializeField] private List<UnityEngine.UI.Button> remapButtons;
    [SerializeField] private PopUpMenu popUpMenu;

    [Header("Parameters")]
    [SerializeField] private float rebindTimeDelay = 0.25f;

    [Header("Sounds")]
    [SerializeField] private string finishRemapSound;
    [SerializeField] private string enterRemapSound;

    private InputRemapping inputRemapping;

    protected override void Configure()
    {
        inputRemapping = new();
        inputRemapping.startRemap += ActivatePopUp;
        inputRemapping.endRemap += DeactivatePopUp;

        rumble.Value = GameManager.Save.Options.rumble;
        remapButtons.ForEach(button => button.onClick.AddListener(delegate { Remapping(button.name); } ));
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        GameManager.Input.UpdateInputMapping();
    }

    private void ActivatePopUp()
    {
        GameManager.Audio.Play(enterRemapSound);
        popUpMenu.PopUpMessage("Waiting for input...");
    }

    private void DeactivatePopUp()
    {
        popUpMenu.DisablePopUpMenu();
        GameManager.Audio.Play(finishRemapSound);
    }

    public void Rumble(bool value)
    {
        Toggle(ref GameManager.Save.Options.rumble, value);
    }

    public void Remapping(string name)
    {
        inputRemapping.Remapping(rebindTimeDelay, name);
    }
}
