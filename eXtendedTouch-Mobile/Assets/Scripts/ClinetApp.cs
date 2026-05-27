using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System;
using XT;


public class ClinetApp : MonoBehaviour
{
    private XtTcpClient _client;
    [SerializeField] private ArManager _arManager;


    void OnEnable()
    {
        _client = XtTcpClient.Instance;

        EnhancedTouchSupport.Enable();

        _client.MessageSent += HandleMessageSent;
    }

    void OnDisable()
    {
        if (_client == null) return;

        EnhancedTouchSupport.Disable();

        _client.MessageSent -= HandleMessageSent;
    }


    void Update()
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        string message = timestamp.ToString();
        _client.Write(message);
    }


    private void HandleMessageSent(string message)
    {
        Debug.Log($"[CLIENT] Sent: {message}");
    }

}
