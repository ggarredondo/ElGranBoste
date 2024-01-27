using UnityEngine;
using TMPro;

public class TextAutoSet : MonoBehaviour
{
    [SerializeField] private bool inputButton;
    [SerializeField] private TMP_Text tmpText;

    private int playerIndex;

    private void Start()
    {
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

    public void SetPlayerIndex(int index)
    {
        playerIndex = index;
    }

    private void ChangeInputFont()
    {
        tmpText.font = GlobalMenuVariables.Instance.inputFonts[GameManager.Input.CheckControlScheme(playerIndex)];
        tmpText.fontSize = GlobalMenuVariables.Instance.inputFontSize[GameManager.Input.CheckControlScheme(playerIndex)];

        string mappingKey = GameManager.Input.ObtainMapping(tmpText.gameObject.name, playerIndex);

        if (mappingKey != "-" && mappingKey != "") tmpText.text = mappingKey;
        else
        {
            tmpText.font = GlobalMenuVariables.Instance.inputFonts[0];
            tmpText.text = "M";
        }
    }
}
