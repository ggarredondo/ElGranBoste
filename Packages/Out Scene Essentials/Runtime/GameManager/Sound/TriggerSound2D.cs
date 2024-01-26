using UnityEngine;

namespace AudioUtilities
{
    public class TriggerSound2D : MonoBehaviour
    {
        [SerializeField] private string enterSound;
        [SerializeField] private string exitSound;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameManager.Audio.Play(enterSound);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            GameManager.Audio.Play(exitSound);
        }
    }
}
