using System.Collections.Generic;
using UnityEngine;

namespace SaveUtilities
{
    [System.Serializable]
    public class GameSlot : SaveSlot
    {
        public float lastScore;

        public new object Clone()
        {
            GameSlot nuevo = (GameSlot)MemberwiseClone();

            return nuevo;
        }
    }
}
