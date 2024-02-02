using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class RandomizePositions : MonoBehaviour
{
    [SerializeField] private CharacterController player;
    [SerializeField] private NavMeshAgent enemy;
    [SerializeField] private Transform bookPoste;
    [SerializeField] private List<Transform> books;
    [SerializeField] private float numberOfSpawnedBooks;
    private List<Transform> spawnPoints;
    private System.Random rng;

    private int FurthestIndexFromPlayer()
    {
        int index = -1;
        float furthestDistance = float.MinValue;
        for (int i = 0; i < spawnPoints.Count; ++i)
        {
            if (Vector3.Distance(player.transform.position, spawnPoints[i].position) > furthestDistance)
            {
                index = i;
                furthestDistance = Vector3.Distance(player.transform.position, spawnPoints[i].position);
            }
        }
        return index;
    }

    private void Start()
    {
        // Initialize spawn points
        spawnPoints = new List<Transform>();
        for (int i = 0; i < transform.childCount; ++i)
            spawnPoints.Add(transform.GetChild(i).transform);

        // Place player
        rng = new System.Random();
        int playerIndex = rng.Next(0, spawnPoints.Count);
        player.enabled = false;
        player.transform.position = spawnPoints[playerIndex].position + Vector3.up * 1.5f;
        player.enabled = true;
        spawnPoints.Remove(spawnPoints[playerIndex]);

        // Place enemy in the furthest point away from player
        int enemyIndex = FurthestIndexFromPlayer();
        enemy.Warp(spawnPoints[enemyIndex].position);
        spawnPoints.Remove(spawnPoints[enemyIndex]);

        // Place poste in the furthest remaining point away from player
        int posteIndex = FurthestIndexFromPlayer();
        bookPoste.position = spawnPoints[posteIndex].position + Vector3.up;
        spawnPoints.Remove(spawnPoints[posteIndex]);

        // Place books randomly
        Debug.Assert(numberOfSpawnedBooks < books.Count, "Number of spawned books " +
            "must be lower than the number of total books");
        int bookIndex, posIndex;
        for (int i = 0; i < numberOfSpawnedBooks; ++i)
        {
            bookIndex = rng.Next(0, books.Count);
            posIndex = rng.Next(0, spawnPoints.Count);
            books[bookIndex].position = spawnPoints[posIndex].position + Vector3.up;
            books.Remove(books[bookIndex]);
            spawnPoints.Remove(spawnPoints[posIndex]);
        }
    }
}
