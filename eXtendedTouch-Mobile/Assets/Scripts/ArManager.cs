using UnityEngine;
using UnityEngine.XR.ARFoundation;
using System;


public class ArManager : MonoBehaviour
{
    [SerializeField] private ARSession _arSession;
    [SerializeField] private Camera _arCamera;

    private string _session;
    public string Session
    {
        get { return _session; }
    }

    private Vector3 _position;
    public Vector3 Position
    {
        get { return _position; }
    }

    private Quaternion _rotation;
    public Quaternion Rotation
    {
        get { return _rotation; }
    }


    void Start()
    {
        _session = "N/A";
        _position = Vector3.zero;
        _rotation = Quaternion.identity;
    }


    void Update()
    {
        if (_arSession == null || _arCamera == null) return;

        _session = ARSession.state.ToString();
        _position = _arCamera.transform.position;
        _rotation = _arCamera.transform.rotation;
    }
}
