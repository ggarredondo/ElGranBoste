using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventHandler", menuName = "Scriptable Objects/Events/EventHandler")]
public class EventHandler : ScriptableObject
{
    [SerializeField] private List<string> eventNames;

    public Dictionary<string, System.Action> events;
    public Dictionary<string, System.Action<GameObject>> gameObjectEvents;

    public void Initialize()
    {
        events = new();
        gameObjectEvents = new();

        eventNames.ForEach(a => events.Add(a, null));
        eventNames.ForEach(a => gameObjectEvents.Add(a, null));
    }

    public void OnInvoke(string name)
    {
        events[name]?.Invoke();
    }

    public void OnGameObjectInvoke(string name, GameObject gameObject)
    {
        gameObjectEvents[name]?.Invoke(gameObject);
    }
}
