using UnityEngine;

public enum JokeType { Stun, Boste }

[System.Serializable]
public class Joke
{
    [SerializeField] private string sentence;
    [SerializeField] private float timeToPerform, laughingTime;
    [SerializeField] private JokeType type;

    public string Sentence => sentence;
    public float TimeToPerform => timeToPerform;
    public float LaughingTime => laughingTime;
    public JokeType Type => type;
}
