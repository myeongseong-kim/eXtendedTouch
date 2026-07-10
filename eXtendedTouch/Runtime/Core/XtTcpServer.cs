using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using XT;


namespace XT {

public class XtTcpServer : MonoBehaviour
{
    public static XtTcpServer Instance { get; private set; }

    private TcpListener _listener;
    private UdpClient _discoveryUdp;
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
    public (string ip, int port) ServerAddress => (_serverIp, _port);

    public bool IsConnected => _client != null && _reader != null && _writer != null;


    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _serverIp = XtNetwork.GetLocalIPv4();
        _port = XtNetwork.XT_PORT;
    }


    void Start()
    {
        StartServer();
        StartDiscovery();

        Debug.Log($"[SERVER] Listening on {_serverIp}:{_port}");
    }


    void Update()
    {
        if (_listener == null) return;

        HandleDiscovery();

        if (_client == null)
        {
            Accept();
            return;
        }

        if (ValidateConnection())
        {
            Read();
        }
    }


    void OnApplicationQuit()
    {
        StopServer();
    }

    private void StartDiscovery()
    {
        try
        {
            _discoveryUdp = new UdpClient();
            _discoveryUdp.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
            _discoveryUdp.Client.Bind(new IPEndPoint(IPAddress.Any, _port));

            Debug.Log($"[SERVER] UDP discovery listening on port {_port}");
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
            StopDiscovery();
        }
    }

    private void HandleDiscovery()
    {
        if (_discoveryUdp == null) return;

        try
        {
            while (_discoveryUdp.Available > 0)
            {
                IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] requestBytes = _discoveryUdp.Receive(ref remoteEndPoint);
                string request = Encoding.UTF8.GetString(requestBytes);

                if (request != XtNetwork.XT_DISCOVERY_REQUEST) continue;

                string response = _serverIp;
                byte[] responseBytes = Encoding.UTF8.GetBytes(response);
                _discoveryUdp.Send(responseBytes, responseBytes.Length, remoteEndPoint);

                Debug.Log($"[SERVER] Discovery response sent to {remoteEndPoint.Address}:{remoteEndPoint.Port}");
            }
        }
        catch (Exception e)
        {
            OnErrorOccurred(e);
        }
    }

    private void StopDiscovery()
    {
        _discoveryUdp?.Close();
        _discoveryUdp = null;
    }


    private bool ValidateConnection()
    {
        try
        {
            Socket socket = _client.Client;

            if (socket.Poll(0, SelectMode.SelectRead) && socket.Available == 0)
            {
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
            if (_client.Available <= 0) return;

            string line = _reader.ReadLine();

            if (line == null)
            {
                Disconnect();
                return;
            }

            OnMessageReceived(line);
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


    private void Accept()
    {
        try
        {
            if (!_listener.Pending()) return;

            _client = _listener.AcceptTcpClient();

            var remoteEndPoint = _client.Client.RemoteEndPoint as IPEndPoint;
            if (remoteEndPoint != null)
            {
                _clientIp = remoteEndPoint.Address.ToString();
            }

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

    private void StartServer()
    {
        _listener = new TcpListener(IPAddress.Any, _port);
        _listener.Start();
    }

    private void StopServer()
    {
        Disconnect();

        _listener?.Stop();
        _listener = null;

        StopDiscovery();
    }


    private void OnConnected()
    {
        Connected?.Invoke();
        Debug.Log("[SERVER] Client connected");
    }

    private void OnDisconnected()
    {
        Disconnected?.Invoke();
        Debug.Log("[SERVER] Client disconnected");
    }

    private void OnMessageReceived(string message)
    {
        MessageReceived?.Invoke(message);
        // Debug.Log($"[SERVER] Message received: {message}");
    }

    private void OnMessageSent(string message)
    {
        MessageSent?.Invoke(message);
        // Debug.Log($"[SERVER] Message sent: {message}");
    }

    private void OnErrorOccurred(Exception e)
    {
        ErrorOccurred?.Invoke(e);
        Debug.LogWarning($"[SERVER] Unexpected error: {e.Message}");
    }

}

} // namespace XT
