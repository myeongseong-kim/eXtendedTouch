using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class XtTcpClient : MonoBehaviour
{
    private TcpClient _client;
    private StreamWriter _writer;

    [SerializeField] private string _ip = "127.0.0.1";


    void Start()
    {
        
    }


    void Update()
    {
        if (_writer != null)
        {
            PoseData data = new PoseData
            {
                pose = Pose.identity
            };

            string json = JsonUtility.ToJson(data);
            _writer.WriteLine(json);
        }
    }


    void OnApplicationQuit()
    {
        _writer?.Close();
        _client?.Close();
    }


    public void SetIP(string ip)
    {
        _ip = ip;
    }

    public void Connect()
    {
        if (_client == null)
        {
            int port = XtUtils.XT_PORT;

            _client = new TcpClient(_ip, port);
            _writer = new StreamWriter(_client.GetStream());
            _writer.AutoFlush = true;

            Debug.Log($"[CLIENT] Connected on port {port}");
        }
    }
}
