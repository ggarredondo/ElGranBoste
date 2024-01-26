using UnityEngine;

[System.Serializable]
public class PlayerData : System.ICloneable
{
    [System.NonSerialized] public int deviceID;
    [System.NonSerialized] public string controlScheme;
    [System.NonSerialized] public Vector2 initPosition;

    [Header("Parameters")]
    public Team team;

    [Header("Color")]
    public Color defaultMainColor;
    public Color defaultBoostColor;

    public Color mainColor;
    public Color boostColor;

    public object Clone()
    {
        return MemberwiseClone();
    }
}

public enum Team
{
    NONE,
    TEAM1,
    TEAM2
}
