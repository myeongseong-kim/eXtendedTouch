using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using XT;


public class ClientSceneManager : MonoBehaviour
{
    XtTcpClient _client;

    private Coroutine _bindCoroutine;
    private bool _isBound = false;


    void OnEnable()
    {
        if (_isBound || _bindCoroutine != null) return;

        _bindCoroutine = StartCoroutine(BindClient());
    }

    void OnDisable()
    {
        if (_bindCoroutine != null)
        {
            StopCoroutine(_bindCoroutine);
            _bindCoroutine = null;
        }

        UnbindClient();
    }


    private IEnumerator BindClient()
    {
        while (XtTcpClient.Instance == null)
        {
            yield return null;
        }
        _client = XtTcpClient.Instance;

        _client.Connected += HandleConnected;
        _client.Disconnected += HandleDisconnected;

        _isBound = true;
        _bindCoroutine = null;
    }

    private void UnbindClient()
    {
        if (_client == null) return;

        _client.Connected -= HandleConnected;
        _client.Disconnected -= HandleDisconnected;

        _isBound = false;
        _bindCoroutine = null;
    }


    private void HandleConnected()
    {
        Debug.Log("Load ClientMainScene");
        SceneManager.LoadScene("ClientMainScene");
    }

    private void HandleDisconnected()
    {
        Debug.Log("Load ClientConnectionScene");
        SceneManager.LoadScene("ClientConnectionScene");
    }
}
