using UnityEngine;
using System;
using TMPro;
using XT;


namespace XT {

public class ServerDisplay : MonoBehaviour
{
    XtTcpServer _server;
    [SerializeField] private TMP_Text _serverText;


    void Start()
    {
        _server = XtTcpServer.Instance;

        var address = _server.ServerAddress;
        _serverText.text = $"{address.ip}";
    }

}

} // namespace XT
