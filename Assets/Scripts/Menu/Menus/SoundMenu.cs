using UnityEngine;

public class SoundMenu : AbstractMenu
{
    [Header("UI Elements")]
    [SerializeField] private MySlider master;
    [SerializeField] private MySlider music;
    [SerializeField] private MySlider sfx;
    [SerializeField] private MyToggle mute;

    protected override void Configure()
    {
        mute.Value = GameManager.Save.Options.mute;
        master.Value = GameManager.Save.Options.masterVolume;
        music.Value = GameManager.Save.Options.musicVolume;
        sfx.Value = GameManager.Save.Options.sfxVolume;
    }

    public void ChangeMasterVolume(float value)
    {
        Slider(ref GameManager.Save.Options.masterVolume, value);
    }

    public void ChangeMusicVolume(float value)
    {
        Slider(ref GameManager.Save.Options.musicVolume, value);
    }

    public void ChangeSfxVolume(float value)
    {
        Slider(ref GameManager.Save.Options.sfxVolume, value);
    }

    public void Mute(bool value)
    {
        Toggle(ref GameManager.Save.Options.mute, value);
    }
}
