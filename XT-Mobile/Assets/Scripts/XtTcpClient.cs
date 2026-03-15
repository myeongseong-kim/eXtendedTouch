using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;


public class XtTcpClient : MonoBehaviour
{
    public static XtTcpClient Instance { get; private set; }

    private TcpClient _client;
    private StreamWriter _writer;

    [SerializeField] private string _ip = "127.0.0.1";

    public bool IsConnected => _client != null && _client.Connected;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    void Start()
    {

    }


    void Update()
    {
        // if (_writer != null)
        // {
        //     PoseData data = new PoseData
        //     {
        //         pose = Pose.identity
        //     };

        //     string json = JsonUtility.ToJson(data);
        //     _writer.WriteLine(json);
        // }
    }


    void OnApplicationQuit()
    {
        _writer?.Close();
        _client?.Close();
    }


    public void Write(string message)
    {
        if (_writer != null)
        {
            _writer.WriteLine(message);
        }
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
