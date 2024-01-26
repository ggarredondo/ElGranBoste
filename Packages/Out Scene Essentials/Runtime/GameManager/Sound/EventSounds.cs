using System.Collections.Generic;
using UnityEngine;

public class EventSounds : MonoBehaviour
{
    [SerializeField] private List<EventSoundData> eventSoundData;

    private void Start()
    {
        foreach (EventSoundData data in eventSoundData)
            data.Initialize();
    }
}
