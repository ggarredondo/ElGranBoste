using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTimer : MonoBehaviour
{
    private float startTime;

    private void Awake()
    {
        StartCronometer();
    }

    private void StartCronometer()
    {
        startTime = Time.time;
    }

    private void OnDestroy()
    {
        StopCronometer();
    }

    private void StopCronometer()
    {
        float elapsedTime = Time.time - startTime;

        GameManager.Save.Game.lastScore = elapsedTime;
    }
}
