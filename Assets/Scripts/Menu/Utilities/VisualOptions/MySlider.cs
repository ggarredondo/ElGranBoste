using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[System.Serializable]
public class MySlider : MySelectable, ITransition
{
    [SerializeField] private Slider slider;
    [SerializeField] private UnityEvent<float> trigger;
    [SerializeField] private Color32 activeColor;
    [SerializeField] private Color32 inactiveColor;
    [SerializeField] private string buttonSoundName;
    [SerializeField] private string changeSoundName;
    [SerializeField] private float timeBetweenSounds;

    private UnityAction<float> changeValue;
    private UnityAction transition;

    private bool isPlayingSlider;

    public float Value { get => slider.value; set => slider.value = value; }

    private void SetActions()
    {
        changeValue = (float value) => { trigger.Invoke(value); if (!isPlayingSlider) PlayChangeSound(); };
        transition = () =>
        {
            EventSystem.current.SetSelectedGameObject(slider.gameObject);
            GameManager.Audio.Play(buttonSoundName);
        };
    }

    public override void Initialize()
    {
        SetActions();
        base.Initialize();
        AddListener();
    }

    private async void PlayChangeSound()
    {
        GameManager.Audio.Play(changeSoundName);
        isPlayingSlider = true;
        await Task.Delay(System.TimeSpan.FromSeconds(timeBetweenSounds));
        isPlayingSlider = false;
    }

    public override void AddListener()
    {
        slider.onValueChanged.AddListener(changeValue);
    }

    public override void RemoveListener()
    {
        slider.onValueChanged.RemoveListener(changeValue);
    }

    public override void SetButtonAction()
    {
        button.onClick.AddListener(transition);
    }

    public override void SetDependency()
    {
        dependency.onValueChanged.AddListener((bool value) =>
        {
            if(inverseDependency)
                value = !value;

            if (value)
            {
                ChangeColor(activeColor);
                button.onClick.AddListener(transition);
                slider.interactable = true;
            }
            else
            {
                ChangeColor(inactiveColor);
                button.onClick.RemoveListener(transition);
                slider.interactable = false;
            }
        });
    }

    public override void ChangeColor(Color32 color)
    {
        base.ChangeColor(color);

        ColorBlock colors = slider.colors;
        colors.disabledColor = color;
        slider.colors = colors;
    }

    public void Return()
    {
        EventSystem.current.SetSelectedGameObject(button.gameObject);
    }

    public bool HasTransition()
    {
        bool transition = EventSystem.current.currentSelectedGameObject == slider.gameObject;

        if (transition) Return();

        return transition;
    }
}
