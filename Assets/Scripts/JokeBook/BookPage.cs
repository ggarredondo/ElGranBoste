using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class BookPage : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private TMP_Text textMeshPro;

    [Header("Parameters")]
    [SerializeField] float pageRotationAngle;
    [SerializeField] float initialPageRotationAngle;
    [SerializeField] float pageRotationTime;

    public void SetStyle(string sentence)
    {
        textMeshPro.text = sentence;
    }

    public void MoveForward()
    {
        transform.DORotate(new Vector3(0, 0, pageRotationAngle), pageRotationTime);
    }

    public void MoveBackWards()
    {
        transform.DORotate(new Vector3(0, 0, initialPageRotationAngle), pageRotationTime);
    }
}
