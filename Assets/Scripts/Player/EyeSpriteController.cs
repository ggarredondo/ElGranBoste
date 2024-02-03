using UnityEngine;
using UnityEngine.UI;

public class EyeSpriteController : MonoBehaviour
{
    [SerializeField] private PlayerStateMachine player;
    [SerializeField] private Sprite openEye, closedEye;
    private Image image;

    private void Awake() => image = GetComponent<Image>();
    void Update() => image.sprite = player.IsEnemyInRange() ? openEye : closedEye;
}
