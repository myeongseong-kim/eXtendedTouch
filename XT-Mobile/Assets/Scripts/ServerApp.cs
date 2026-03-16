using UnityEngine;
using System;
using TMPro;


public class ServerApp : MonoBehaviour
{
    private XtTcpServer _server;
    [SerializeField] private XtManager _xtManager;

    [SerializeField] private TMP_Text _dataLog;


    void OnEnable()
    {
        _server = XtTcpServer.Instance;

        _server.MessageReceived += HandleMessageReceived;
    }

    void OnDisable()
    {
        if (_server == null) return;

        _server.MessageReceived -= HandleMessageReceived;
    }


    void Update()
    {

    }


    private void HandleMessageReceived(string message)
    {
        Debug.Log($"[SERVER] Received: {message}");

        var data = JsonUtility.FromJson<XtMessage>(message);

        string session = data.arFrame.session;
        Vector3 pos = data.arFrame.position;
        Quaternion rot = data.arFrame.rotation;

        _dataLog.text =
            $"State: {session}\n" +
            $"Position: {pos.x:F3}, {pos.y:F3}, {pos.z:F3}\n" +
            $"Rotation: {rot.x:F3}, {rot.y:F3}, {rot.z:F3}, {rot.w:F3}\n";

        _xtManager.SetTransform(pos, rot);
    }

}
