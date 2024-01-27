using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AutoSelect : MonoBehaviour, IPointerEnterHandler, ISelectHandler, IDeselectHandler, IPointerExitHandler
{
    [SerializeField] private int typeButton;
    [SerializeField] private bool mouseCanSelect;
    [SerializeField] private bool inputButton;
    [SerializeField] private GameObject enableObject;

    private Selectable selectable;

    private void Start()
    {
        //Initialize font
        GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.font;
    }

    private void OnEnable()
    {
        if (enableObject != null)
            enableObject.GetComponent<Image>().color = GlobalMenuVariables.Instance.playerColor;

        ColorBlock colors = selectable.colors;

        switch (typeButton)
        {
            case 1:
                colors.selectedColor = GlobalMenuVariables.Instance.selectedButtonColor;
                colors.highlightedColor = GlobalMenuVariables.Instance.highlightedButtonColor;
                colors.normalColor = GlobalMenuVariables.Instance.normalButtonColor;
                break;
            case 2:
                colors.selectedColor = GlobalMenuVariables.Instance.selectedButtonColor2;
                colors.highlightedColor = GlobalMenuVariables.Instance.highlightedButtonColor2;
                colors.normalColor = GlobalMenuVariables.Instance.normalButtonColor2;
                break;
        }

        selectable.colors = colors;
    }

    private void Awake()
    {
        selectable = GetComponent<Selectable>();

        if (inputButton)
        {
            GameManager.Input.ControlsChangedEvent += ChangeInputFont;
            ChangeInputFont();
        }
    }

    private void OnDestroy()
    {
        if (inputButton) GameManager.Input.ControlsChangedEvent -= ChangeInputFont;
    }

    public void OnSelect(BaseEventData eventData)
    { 
        GameManager.Input.SetSelectedGameObject(EventSystem.current.currentSelectedGameObject);
        GameManager.Audio.Play("SelectButton");

        if (enableObject != null)
            enableObject.SetActive(true);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (enableObject != null)
            enableObject.SetActive(false);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (mouseCanSelect)
        {
            EventSystem.current.SetSelectedGameObject(null);
            if (enableObject != null)
                enableObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (mouseCanSelect)
        {
            EventSystem.current.SetSelectedGameObject(gameObject);
            GameManager.Audio.Play("SelectButton");

            if (enableObject != null)
                enableObject.SetActive(true);
        }
    }

    private void ChangeInputFont()
    {
        //Change Font
        transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[GameManager.Input.ControlSchemeIndex];

        //Asign the correct word
        string mappingKey = GameManager.Input.ObtainAllowedMapping(gameObject.name);

        if (mappingKey != "-" && mappingKey != "") transform.GetChild(0).GetComponent<TMP_Text>().text = mappingKey;
        else
        {
            transform.GetChild(0).GetComponent<TMP_Text>().font = GlobalMenuVariables.Instance.inputFonts[0];
            transform.GetChild(0).GetComponent<TMP_Text>().text = "M";
        }
    }
}
