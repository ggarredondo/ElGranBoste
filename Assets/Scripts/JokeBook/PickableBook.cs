using DG.Tweening;
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

    [Header("Sounds")]
    [SerializeField] private string ambientSoundName;
    [SerializeField] private string pickUpSoundName;

    private Sequence sequence;

    private void Start()
    {
        GameManager.Audio.Play(ambientSoundName);
        sequence = DOTween.Sequence();
        Move();
    }

    private void Move()
    {
        sequence.Append(transform.DOMove(transform.position + positionOffset, animationTime));
        sequence.SetLoops(-1, LoopType.Yoyo);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine player = other.gameObject.GetComponent<PlayerStateMachine>();

            foreach(Joke joke in jokes)
            {
                player.JokeList.Add(joke);
            }

            player.OnPickBook.Invoke();

            sequence.Kill();

            GameManager.Audio.Play(pickUpSoundName);

            Destroy(gameObject, 0.5f);
        }
    }
}
