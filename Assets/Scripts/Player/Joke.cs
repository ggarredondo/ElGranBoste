using UnityEngine;

public enum JokeType { Stun, Boste }
public class Joke
{
    private string sentence;
    private float timeToPerformMS, laughingTimeMS;
    private JokeType type;
}
