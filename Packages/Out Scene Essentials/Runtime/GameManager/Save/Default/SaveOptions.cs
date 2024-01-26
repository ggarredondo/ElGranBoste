using UnityEngine;

namespace SaveUtilities
{
    [CreateAssetMenu(fileName = "SaveConfig", menuName = "Scriptable Objects/Save/SaveOptions")]
    public class SaveOptions : ScriptableObject
    {
        public OptionsSlot defaultOptions;
    }
}
