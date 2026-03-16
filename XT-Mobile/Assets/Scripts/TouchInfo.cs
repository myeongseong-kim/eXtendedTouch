using UnityEngine;
using System;


[Serializable]
public struct TouchInfo
{
    public int touchId;
    public Vector2 screenPosition;
    public Vector2 delta;
    public string phase;
}
