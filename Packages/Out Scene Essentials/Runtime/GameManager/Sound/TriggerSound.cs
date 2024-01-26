using UnityEngine;

namespace AudioUtilities
{
    public class TriggerSound : MonoBehaviour
    {
        [SerializeField] private string sound;

        private void OnTriggerEnter(Collider other)
        {
            GameManager.Audio.Play(sound);
        }
    }
}
