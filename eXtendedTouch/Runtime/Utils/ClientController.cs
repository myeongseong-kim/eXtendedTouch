using UnityEngine;
using System;
using XT;


namespace XT {

public class ClientController : MonoBehaviour
{
    private XtTcpClient _client;


    void Start()
    {
        _client = XtTcpClient.Instance;
    }


    public void SetServerIp(string ip)
    {
        _client.SetServerIp(ip);
    }

    public void Connect()
    {
        _client.Connect();
    }
}

} // namespace XT
