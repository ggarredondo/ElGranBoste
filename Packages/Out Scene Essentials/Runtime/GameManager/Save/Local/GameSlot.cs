using System.Collections.Generic;
using UnityEngine;

namespace SaveUtilities
{
    [System.Serializable]
    public class GameSlot : SaveSlot
    {
        [Header("Player")]
        public List<int> availableSouls;
        public List<int> availableDestinies;
        public List<int> currentSoulDeck;
        public List<int> currentDestinyDeck;

        public new object Clone()
        {
            GameSlot nuevo = (GameSlot)MemberwiseClone();

            nuevo.availableSouls = new List<int>(availableSouls);
            nuevo.availableDestinies = new List<int>(availableDestinies);
            nuevo.currentSoulDeck = new List<int>(currentSoulDeck);
            nuevo.currentDestinyDeck = new List<int>(currentDestinyDeck);

            return nuevo;
        }
    }
}
