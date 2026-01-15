using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class TestServer : MonoBehaviour
{
    TcpListener _listener;
    TcpClient _client;
    StreamReader _reader;

    public int port = 5000;


    void Start()
    {
        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Debug.Log($"[SERVER] Listening on port {port}");
    }


    void Update()
    {
        if (_client == null && _listener.Pending())
        {
            _client = _listener.AcceptTcpClient();
            _reader = new StreamReader(_client.GetStream());
            Debug.Log("Client connected");
        }

        if (_reader != null && _client.Available > 0)
        {
            string line = _reader.ReadLine();
            PoseData data = JsonUtility.FromJson<PoseData>(line);
            Debug.Log($"Pose: {data.pose.position}, {data.pose.rotation}");
        }
    }


    void OnApplicationQuit()
    {
        _reader?.Close();
        _client?.Close();
        _listener.Stop();
    }
}
