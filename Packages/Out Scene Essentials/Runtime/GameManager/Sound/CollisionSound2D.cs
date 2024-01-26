using System.Collections;
using UnityEngine;

public class CollisionSound2D : MonoBehaviour
{
    [SerializeField] private string enterSound;
    [SerializeField] private float timeBetweenSounds;

    private bool canPlaySound = true;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PlaySound();
    }

    public void PlaySound()
    {
        if (canPlaySound)
            StartCoroutine(HitEffectWithTime());
    }

    public IEnumerator HitEffectWithTime()
    {
        GameManager.Audio.Play(enterSound);

        canPlaySound = false;

        yield return new WaitForSeconds(timeBetweenSounds);

        canPlaySound = true;
    }
}
