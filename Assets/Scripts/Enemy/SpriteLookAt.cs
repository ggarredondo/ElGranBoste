using UnityEngine;

public class SpriteLookAt : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    private void Start() => playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    private void Update() => transform.LookAt(playerTransform);
}
