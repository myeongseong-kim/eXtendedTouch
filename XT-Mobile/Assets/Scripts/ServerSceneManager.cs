using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;


public class ServerSceneManager : MonoBehaviour
{
    XtTcpServer _server;

    private Coroutine _bindCoroutine;
    private bool _isBound = false;


    void OnEnable()
    {
        if (_isBound || _bindCoroutine != null) return;

        _bindCoroutine = StartCoroutine(BindServer());
    }

    void OnDisable()
    {
        if (_bindCoroutine != null)
        {
            StopCoroutine(_bindCoroutine);
            _bindCoroutine = null;
        }

        UnbindServer();
    }


    private IEnumerator BindServer()
    {
        while (XtTcpServer.Instance == null)
        {
            yield return null;
        }
        _server = XtTcpServer.Instance;

        _server.Connected += HandleConnected;
        _server.Disconnected += HandleDisconnected;

        _isBound = true;
        _bindCoroutine = null;
    }

    private void UnbindServer()
    {
        if (_server == null) return;

        _server.Connected -= HandleConnected;
        _server.Disconnected -= HandleDisconnected;

        _isBound = false;
        _bindCoroutine = null;
    }


    private void HandleConnected()
    {
        Debug.Log("Load ServerMainScene");
        SceneManager.LoadScene("ServerMainScene");
    }

    private void HandleDisconnected()
    {
        Debug.Log("Load ServerConnectionScene");
        SceneManager.LoadScene("ServerConnectionScene");
    }
}
