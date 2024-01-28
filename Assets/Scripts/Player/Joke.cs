using UnityEngine;

public enum JokeType { Stun, Poste }

[System.Serializable]
public class Joke
{
    [SerializeField] private string id, sentence, sfx;
    [SerializeField] private float timeToPerform, laughingTime;
    [SerializeField] private JokeType type;

    public string ID => id;
    public string Sentence => sentence;
    public string SFX => sfx;
    public float TimeToPerform => timeToPerform;
    public float LaughingTime => laughingTime;
    public JokeType Type => type;
}
