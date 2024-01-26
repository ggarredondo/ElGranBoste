using UnityEngine;

namespace SaveUtilities
{
    [CreateAssetMenu(fileName = "SaveConfig", menuName = "Scriptable Objects/Save/SaveGame")]
    public class SaveGame : ScriptableObject
    {
        public int numGameSlots;

        public GameSlot defaultGame;
    }
}
