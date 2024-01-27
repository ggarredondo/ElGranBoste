using UnityEngine;
using TMPro;

public class GlobalMenuVariables : MonoBehaviour
{
    public static GlobalMenuVariables Instance { get; private set; }

    [Header("Font Asset")]
    public TMP_FontAsset font;
    public TMP_FontAsset[] inputFonts;
    public float[] inputFontSize;

    [Header("Player Color")]
    public Color32 playerColor;

    [Header ("Button type 1")]
    public Color32 normalButtonColor;
    public Color32 selectedButtonColor;
    public Color32 highlightedButtonColor;

    [Header("Button type 2")]
    public Color32 normalButtonColor2;
    public Color32 selectedButtonColor2;
    public Color32 highlightedButtonColor2;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
}
