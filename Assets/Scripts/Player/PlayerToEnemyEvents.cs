using UnityEngine;
using RefDelegates;

public class PlayerToEnemyEvents : MonoBehaviour
{
    public System.Action OnJokeStart, OnJokeCancelled;
    public ActionIn<Joke> OnJokePerformed;
    public System.Action OnParry;

    public System.Action OnKillPlayer;
}
