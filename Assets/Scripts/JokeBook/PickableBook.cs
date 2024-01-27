using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableBook : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private List<Joke> jokes;

    [Header("Parameters")]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Quaternion rotationOffset;
    [SerializeField] private float animationTime;

    private void Start()
    {
        transform.DOMove(transform.position + positionOffset, animationTime).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            Debug.Log(other.gameObject.name);
            PlayerStateMachine player = other.gameObject.GetComponent<PlayerStateMachine>();

            foreach(Joke joke in jokes)
            {
                player.JokeList.Add(joke);
            }

            player.OnPickBook.Invoke();

            transform.DOKill();
            Destroy(gameObject);
        }
    }
}
