using UnityEngine;
using System;
using TMPro;
using XT;


public class ServerApp : MonoBehaviour
{
    private XtTcpServer _server;


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
    }

}
