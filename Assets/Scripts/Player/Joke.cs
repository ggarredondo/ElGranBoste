using UnityEngine;

public enum JokeType { Stun, Poste }

[System.Serializable]
public class Joke
{
    [SerializeField] private string id, sentence, sfx;
    private float timeToPerform;
    [SerializeField] private float laughingTime;
    [SerializeField] private JokeType type;

    public void Initialize()
    {
        timeToPerform = GameManager.Audio.Length(sfx);
    }

    public string ID => id;
    public string Sentence => sentence;
    public string SFX => sfx;
    public float TimeToPerform => timeToPerform;
    public float LaughingTime => laughingTime;
    public JokeType Type => type;
}
