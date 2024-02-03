using InputUtilities;
using System.Collections.Generic;
using UnityEngine;

public class ControlsMenu : AbstractMenu
{ 
    [Header("UI Elements")]
    [SerializeField] private MySlider mouseSensitivity;

    protected override void Configure()
    {
        mouseSensitivity.Value = GameManager.Save.Options.mouseSensitivity;
    }

    public void Sensitivity(float value)
    {
        Slider(ref GameManager.Save.Options.mouseSensitivity, value);
    }
}
