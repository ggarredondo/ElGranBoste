using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class PickableBook : MonoBehaviour
{
    [Header("Requirements")]
    [SerializeField] private List<Joke> jokes;

    [Header("Parameters")]
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private Vector3 rotation;
    [SerializeField] private float animationTime;
    [SerializeField] private float animationRotationTime;

    [Header("Sounds")]
    [SerializeField] private string ambientSoundName;
    [SerializeField] private string pickUpSoundName;

    private MeshRenderer meshRenderer;
    private Collider bookCollider;

    private Sequence moveSequence;
    private Sequence rotateSequence;

    private void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        bookCollider = GetComponent<Collider>();

        GameManager.Audio.Play(ambientSoundName);
        Move();
    }

    private void Move()
    {
        moveSequence = DOTween.Sequence();
        moveSequence.Append(transform.DOLocalMove(transform.localPosition + positionOffset, animationTime).SetEase(Ease.InOutQuad));
        moveSequence.SetLoops(-1, LoopType.Yoyo);

        rotateSequence = DOTween.Sequence();
        rotateSequence.Append(transform.DOLocalRotate(rotation, animationRotationTime, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        rotateSequence.SetLoops(-1, LoopType.Restart);
    }

    private void OnDestroy()
    {
        moveSequence.Kill();
        rotateSequence.Kill();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            PlayerStateMachine player = other.gameObject.GetComponent<PlayerStateMachine>();

            foreach(Joke joke in jokes)
            {
                joke.Initialize();
                player.JokeList.Add(joke);
            }

            player.OnPickBook.Invoke();

            DisableBook();
        }
    }

    private void DisableBook()
    {
        GameManager.Audio.Stop(ambientSoundName);
        GameManager.Audio.Play(pickUpSoundName);

        meshRenderer.enabled = false;
        bookCollider.enabled = false;

        Destroy(gameObject, GameManager.Audio.Length(pickUpSoundName));
    }
}
