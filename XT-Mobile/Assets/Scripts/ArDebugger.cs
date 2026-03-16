using UnityEngine;
using System;
using TMPro;


public class ArDebugger : MonoBehaviour
{
    [SerializeField] private ArManager _arManager;
    [SerializeField] private TMP_Text _arLog;


    void Start()
    {
        
    }


    void Update()
    {
        string session = _arManager.Session;
        Vector3 pos = _arManager.Position;
        Quaternion rot = _arManager.Rotation;

        _arLog.text = 
            $"State: {session}\n" + 
            $"Position: {pos.x:F3}, {pos.y:F3}, {pos.z:F3}\n" +
            $"Rotation: {rot.x:F3}, {rot.y:F3}, {rot.z:F3}, {rot.w:F3}\n";
    }
}
