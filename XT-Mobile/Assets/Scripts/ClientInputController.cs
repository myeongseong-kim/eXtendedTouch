using UnityEngine;
using System;


public class ClientInputController : MonoBehaviour
{
    private XtTcpClient _client;


     void Awake()
    {
        _client = XtTcpClient.Instance;
    }


    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public void ClientSetServerIp(string ip)
    {
        _client.SetServerIp(ip);
    }

    public void ClientConnect()
    {
        _client.Connect();
    }
}
