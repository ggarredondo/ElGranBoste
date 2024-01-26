using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventSoundData
{
    [SerializeField] private EventHandler eventHandler;
    [SerializeField] private List<string> soundNames;

    public void Initialize()
    {
        foreach (string name in soundNames)
            eventHandler.events[name] += () => GameManager.Audio.Play(name);
    }
}
