using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using System;


public class ClinetApp : MonoBehaviour
{
    private XtTcpClient _client;
    [SerializeField] private ArManager _arManager;


    void OnEnable()
    {
        _client = XtTcpClient.Instance;

        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        if (_client == null) return;

        EnhancedTouchSupport.Disable();
    }


    void Update()
    {
        ArFrame arFrame = new ArFrame
        {
            session = _arManager.Session,
            position = _arManager.Position,
            rotation = _arManager.Rotation
        };

        var touches = new TouchInfo[Touch.activeTouches.Count];
        for (int i = 0; i < Touch.activeTouches.Count; i++)
        {
            touches[i] = new TouchInfo
            {
                touchId = Touch.activeTouches[i].touchId,
                position = Touch.activeTouches[i].screenPosition,
                delta = Touch.activeTouches[i].delta,
                phase = Touch.activeTouches[i].phase.ToString()
            };
        }
        TouchFrame touchFrame = new TouchFrame
        {
            resolution = new Vector2Int(Screen.width, Screen.height),
            dpi = Screen.dpi > 0 ? (int)Screen.dpi : 72,
            touches = touches
        };

        XtMessage message = new XtMessage
        {
            timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
            arFrame = arFrame, 
            touchFrame = touchFrame
        };

        string json = JsonUtility.ToJson(message);
        _client.Write(json);
    }

}
