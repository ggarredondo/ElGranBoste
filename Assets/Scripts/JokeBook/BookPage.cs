using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using System.Threading.Tasks;

public class BookPage : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private TMP_Text textMeshPro;
    [SerializeField] private TMP_Text textMeshProSize;

    [Header("Parameters")]
    [SerializeField] float pageRotationAngle;
    [SerializeField] float initialPageRotationAngle;
    [SerializeField] float pageRotationTime;

    [Header("Sounds")]
    [SerializeField] private string movePageSoundName;

    public void SetStyle(string sentence, int textSize)
    {
        textMeshPro.text = sentence;
        textMeshProSize.text = textSize.ToString();
    }

    public void MoveForward()
    {
        GameManager.Audio.Play(movePageSoundName);
        transform.DOLocalRotate(new Vector3(0, 0, pageRotationAngle), pageRotationTime);
    }

    public async Task MoveBackWards()
    {
        GameManager.Audio.Play(movePageSoundName);
        await transform.DOLocalRotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime).AsyncWaitForCompletion();
    }
}
