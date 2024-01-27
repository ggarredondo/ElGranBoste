using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BookPage : MonoBehaviour
{
    [SerializeField] private MeshRenderer mesh;
    [SerializeField] private TextMeshPro text;

    public void SetStyle(Material material, string sentence)
    {
        mesh.material = material;
        text.text = sentence;
    }
}
