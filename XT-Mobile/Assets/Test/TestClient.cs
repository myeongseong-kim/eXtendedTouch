using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class TestClient : MonoBehaviour
{
    TcpClient _client;
    StreamWriter _writer;

    public string ip = "127.0.0.1";
    public int port = 5000;


    void Start()
    {
        _client = new TcpClient(ip, port);
        _writer = new StreamWriter(_client.GetStream());
        _writer.AutoFlush = true;
        Debug.Log($"[CLIENT] Connected on port {port}");
    }


    void Update()
    {
        PoseData data = new PoseData
        {
            pose = Pose.identity
        };

        string json = JsonUtility.ToJson(data);
        _writer.WriteLine(json); 
    }


    void OnApplicationQuit()
    {
        _writer.Close();
        _client.Close();
    }
}
