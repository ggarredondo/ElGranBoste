using UnityEngine;

[System.Serializable]
public class TransitionData
{
    public TransitionData(TransitionData data)
    {
        startLeftPosition = data.startLeftPosition;
        startRightPosition = data.startRightPosition;

        endLeftPosition = data.endLeftPosition;
        endRightPosition = data.endRightPosition;

        startSound = data.startSound;
        endSound = data.endSound;
    }

    [Header("Start position")]
    public Vector2 startLeftPosition;
    public Vector2 startRightPosition;

    [Header("End position")]
    public Vector2 endLeftPosition;
    public Vector2 endRightPosition;

    [Header("Sound")]
    public string startSound;
    public string endSound;
}
