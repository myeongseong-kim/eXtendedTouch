using UnityEngine;
using System;
using TMPro;


public class Tag : MonoBehaviour
{
    [SerializeField] private int _id;
    [SerializeField] private TextMeshPro _textMesh;
    public void SetId(int id)
    {
        _id = id;
        _textMesh.text = $"{_id}";
    }


    void Start()
    {

    }

    void Update()
    {

    }
}
