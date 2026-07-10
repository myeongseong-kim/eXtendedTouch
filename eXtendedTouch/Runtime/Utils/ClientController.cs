using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using XT;


namespace XT {

public class ClientController : MonoBehaviour
{
    private XtTcpClient _client;

    [SerializeField] private TMP_InputField _serverIpInputField;
    [SerializeField] private Button _connectButton;


    void Start()
    {
        _client = XtTcpClient.Instance;
    }


    public void SetServerIp(string ip)
    {
        _client.SetServerIp(ip);
    }


    public void Discover()
    {
        if (_client.Discover())
        {
            _serverIpInputField.SetTextWithoutNotify(_client.GetServerIp());
        }
    }

    public void Connect()
    {
        _client.Connect();
    }
}

} // namespace XT
