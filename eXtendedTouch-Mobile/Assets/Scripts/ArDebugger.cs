using UnityEngine;
using System;
using TMPro;


public class ArDebugger : MonoBehaviour
{
    [SerializeField] private ArManager _arManager;
    [SerializeField] private TMP_Text _arSessionLog;
    [SerializeField] private TMP_Text _arPoseLog;


    void Start()
    {
        
    }


    void Update()
    {
        string session = _arManager.Session;
        Vector3 pos = _arManager.Position;
        Quaternion rot = _arManager.Rotation;
        Vector3 euler = rot.eulerAngles;

        _arSessionLog.text = 
            $"[{session}]";

        _arPoseLog.text = 
            $"Position: {pos.x:F3}, {pos.y:F3}, {pos.z:F3}\n" +
            $"Rotation: {euler.x:F3}, {euler.y:F3}, {euler.z:F3}";
    }
}
