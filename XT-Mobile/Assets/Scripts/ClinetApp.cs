using UnityEngine;
using System;


public class ClinetApp : MonoBehaviour
{
    private XtTcpClient _client;
    [SerializeField] private ArManager _arManager;


    void OnEnable()
    {
        _client = XtTcpClient.Instance;
    }


    void Update()
    {
        ArFrame arFrame = new ArFrame
        {
            session = _arManager.Session,
            position = _arManager.Position,
            rotation = _arManager.Rotation
        };

        XtMessage message = new XtMessage
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            arFrame = arFrame
        };

        string json = JsonUtility.ToJson(message);
        _client.Write(json);
    }

}
