using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem.LowLevel;

public class IATest : MonoBehaviour
{
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private Transform targetPosition;

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        agent.SetDestination(targetPosition.position);
    }
}
