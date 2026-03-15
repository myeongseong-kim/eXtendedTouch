using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using TMPro;


public class XtTcpServer : MonoBehaviour
{
    public static XtTcpServer Instance { get; private set; }

    private TcpListener _listener;
    private TcpClient _client;
    private StreamReader _reader;

    [SerializeField] private TMP_Text _serverText;


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
        string ip = XtUtils.GetLocalIPv4();
        int port = XtUtils.XT_PORT;
        _serverText.text = $"{ip}:{port}";

        _listener = new TcpListener(IPAddress.Any, port);
        _listener.Start();
        Debug.Log($"[SERVER] Listening on port {port}");
    }


    void Update()
    {
        if (_client == null && _listener.Pending())
        {
            _client = _listener.AcceptTcpClient();
            var stream = _client.GetStream();
            stream.ReadTimeout = 1;
            _reader = new StreamReader(stream);
            OnConnected();
        }

        if (_client != null && _reader != null)
        {
            if (_client.Client.Poll(0, SelectMode.SelectRead) && _client.Available == 0)
            {
                OnDisconnected();
                return;
            }

            try
            {
                if (_client.Available > 0)
                {
                    string line = _reader.ReadLine();

                    if (line == null)
                    {
                        OnDisconnected();
                        return;
                    }
                    else
                    {
                        HandleData(line);
                    }
                }
            }
            catch (IOException)
            {
                OnDisconnected();
                return;
            }
        }
    }


    void OnApplicationQuit()
    {
        _reader?.Close();
        _client?.Close();
        _listener.Stop();
    }


    private void HandleData(string line)
    {
        try
        {
            PoseData data = JsonUtility.FromJson<PoseData>(line);
            Debug.Log($"Pose: {data.pose.position}, {data.pose.rotation}");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Bad JSON: {line} ({e.Message})");
        }
    }

    private void OnConnected()
    {
        Debug.Log("[SERVER] Client connected");
    }

    private void OnDisconnected()
    {
        Debug.Log("[SERVER] Client disconnected");
        _reader?.Close();
        _client?.Close();
        _reader = null;
        _client = null;
    }
}
