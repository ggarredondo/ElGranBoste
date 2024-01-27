using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VisualsMenu : AbstractMenu
{
    [SerializeField] private VolumeProfile volumeProfile;

    [Header("UI Elements")]
    [SerializeField] private MyToggle fullscreen;
    [SerializeField] private MyToggle vsync;
    [SerializeField] private MyDropdown resolution;
    [SerializeField] private MySlider brightness;
    [SerializeField] private MySlider saturation;

    private ColorAdjustments colorAdjustments;

    protected override void Configure()
    {
        if (volumeProfile.TryGet(out ColorAdjustments tmp)) colorAdjustments = tmp;

        fullscreen.Value = GameManager.Save.Options.fullscreen;
        vsync.Value = GameManager.Save.Options.vSync;
        
        resolution.SetValue(GameManager.Save.Options.resolution);

        brightness.Value = GameManager.Save.Options.brightness;
        saturation.Value = GameManager.Save.Options.saturation;
    }

    public void Vsync(bool value)
    {
        Toggle(ref GameManager.Save.Options.vSync, value);
    }

    public void FullScreen(bool value)
    {
        Toggle(ref GameManager.Save.Options.fullscreen, value);
    }

    public void ChangeResolution(int value)
    {
        Dropdown(ref GameManager.Save.Options.resolution, resolution.GetText(value));
    }

    public void ChangeBrightness(float value)
    {
        GameManager.Save.Options.brightness = value;
        colorAdjustments.postExposure.value = value;
    }

    public void ChangeSaturation(float value)
    {
        GameManager.Save.Options.saturation = value;
        colorAdjustments.saturation.value = value;
    }
}
