using UnityEngine;
using TMPro;

public class ServerDisplay : MonoBehaviour
{
    XtTcpServer _server;
    [SerializeField] private TMP_Text _serverText;

    void Start()
    {
        _server = XtTcpServer.Instance;

        var address = _server.ServerAddress;
        _serverText.text = $"{address.ip}:{address.port}";
    }


    void Update()
    {

    }

}
