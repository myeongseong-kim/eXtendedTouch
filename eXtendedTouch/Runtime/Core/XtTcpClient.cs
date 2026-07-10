using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using XT;


namespace XT {

public class XtTcpClient : MonoBehaviour
{
    public static XtTcpClient Instance { get; private set; }

    private TcpClient _client;
    private StreamReader _reader;
    private StreamWriter _writer;

    public event Action Connected;
    public event Action Disconnected;
    public event Action<string> MessageReceived;
    public event Action<string> MessageSent;
    public event Action<Exception> ErrorOccurred;

    [SerializeField] private string _serverIp;
    [SerializeField] private string _clientIp;
    [SerializeField] private int _port;
    public (string ip, int port) ClientAddress => (_clientIp, _port);

    public bool IsConnected => _client != null && _reader != null && _writer != null;
    private float _DiscoverTimeoutSeconds = 1f;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _clientIp = XtNetwork.GetLocalIPv4();
        _port = XtNetwork.XT_PORT;
    }


    void Start()
    {

    }


    void Update()
    {
        if (_client == null) return;

        if (ValidateConnection())
        {
            Read();
        }
    }


    void OnApplicationQuit()
    {
        Disconnect();
    }


    public bool Discover()
    {
        try
        {
            using (UdpClient discoveryUdp = new UdpClient())
            {
                discoveryUdp.EnableBroadcast = true;
                discoveryUdp.Client.ReceiveTimeout = Mathf.CeilToInt(_DiscoverTimeoutSeconds * 1000f);

                byte[] requestBytes = Encoding.UTF8.GetBytes(XtNetwork.XT_DISCOVERY_REQUEST);
                IPEndPoint broadcastEndPoint = new IPEndPoint(IPAddress.Broadcast, _port);

                discoveryUdp.Send(requestBytes, requestBytes.Length, broadcastEndPoint);
                Debug.Log($"[CLIENT] Discovery request sent on UDP port {_port}");

                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] responseBytes = discoveryUdp.Receive(ref remoteEndPoint);
                string response = Encoding.UTF8.GetString(responseBytes);

                if (!IPAddress.TryParse(response, out _))
                {
                    Debug.LogWarning($"[CLIENT] Discovery ignored invalid response from {remoteEndPoint.Address}");
                    return false;
                }

                _serverIp = response;

                Debug.Log($"[CLIENT] Server discovered at {_serverIp}:{_port}");
                return true;
            }
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            Debug.LogWarning("[CLIENT] Discovery failed: server IP is not found");
            return false;
        }
    }


    public string GetServerIp()
    {
        return _serverIp;
    }

    public void SetServerIp(string ip)
    {
        if (!IPAddress.TryParse(ip, out _))
        {
            Debug.LogWarning($"[CLIENT] Server IP was not changed: '{ip}' is not a valid IP address");
            return;
        }

        _serverIp = ip;
        Debug.Log($"[CLIENT] Server IP set to {_serverIp}");
    }


    private bool ValidateConnection()
    {
        try
        {
            if (_client.Client.Poll(0, SelectMode.SelectRead) && _client.Available == 0)
            {
                Debug.Log("[CLIENT] Server disconnected");
                Disconnect();
                return false;
            }
            return true;
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            Disconnect();
            return false;
        }
    }

    private void Read()
    {
        if (_reader == null) return;

        try
        {
            if (_client.Available > 0)
            {
                string line = _reader.ReadLine();

                if (line == null)
                {
                    Disconnect();
                    return;
                }

                OnMessageReceived(line);
            }
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            Disconnect();
        }
    }

    public void Write(string message)
    {
        if (_writer == null) return;

        try
        {
            _writer.WriteLine(message);
            OnMessageSent(message);
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            Disconnect();
        }
    }

    public void Connect()
    {
        if (IsConnected)
        {
            Debug.LogWarning("[CLIENT] Already connected or connecting");
            return;
        }

        if (!IPAddress.TryParse(_serverIp, out _))
        {
            Debug.LogWarning("[CLIENT] Connect failed: server IP is invalid or not set");
            return;
        }

        try
        {
            _client = new TcpClient(_serverIp, _port);

            var stream = _client.GetStream();
            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream);
            _writer.AutoFlush = true;

            OnConnected();
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            Disconnect();
        }
    }

    public void Disconnect()
    {
        bool wasConnected = _client != null || _reader != null || _writer != null;

        _reader?.Close();
        _writer?.Close();
        _client?.Close();

        _reader = null;
        _writer = null;
        _client = null;

        if (wasConnected)
        {
            OnDisconnected();
        }
    }


    private void OnConnected()
    {
        Connected?.Invoke();
        Debug.Log($"[CLIENT] Connected on {_serverIp}:{_port}");
    }

    private void OnDisconnected()
    {
        Disconnected?.Invoke();
        Debug.Log($"[CLIENT] Client disconnected");
    }

    private void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(message);
        // Debug.Log($"[CLIENT] Message received: {message}");
    }

    private void OnMessageSent(string message)
    {
        MessageSent?.Invoke(message);
        // Debug.Log($"[CLIENT] Message sent: {message}");
    }

    private void OnErrorOccurred(Exception e)
    {
        ErrorOccurred?.Invoke(e);
        Debug.LogWarning($"[CLIENT] Unexpected error: {e.Message}");
    }

}

} // namespace XT
