using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class RandomizePositions : MonoBehaviour
{
    [SerializeField] private Transform player, bookPoste;
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private List<Transform> books;
    private List<Transform> spawnPoints;
    private System.Random rng;

    private void Start()
    {
        // Initialize spawn points
        spawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
            spawnPoints.Add(transform.GetChild(i).transform);

        // Place player
        rng = new System.Random();
        int playerIndex = rng.Next(0, spawnPoints.Count - 1);
        player.position = spawnPoints[playerIndex].position + Vector3.up;
        spawnPoints.Remove(spawnPoints[playerIndex]);

        // Place enemy in the furthest point away from player
        int enemyIndex = -1;
        float furthestDistance = float.MinValue;
        for (int i = 0; i <  spawnPoints.Count; ++i)
        {
            if (Vector3.Distance(player.position, spawnPoints[i].position) > furthestDistance)
            {
                enemyIndex = i;
                furthestDistance = Vector3.Distance(player.position, spawnPoints[i].position);
            }
        }
        Debug.Log(spawnPoints[enemyIndex].position);
        enemy.nextPosition = spawnPoints[enemyIndex].position;
        spawnPoints.Remove(spawnPoints[enemyIndex]);
    }
}
