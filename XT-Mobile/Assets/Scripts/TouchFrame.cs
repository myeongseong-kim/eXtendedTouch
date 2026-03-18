using UnityEngine;
using System;


[Serializable]
public class TouchFrame
{
    public Vector2Int resolution;
    public int dpi;
    public TouchInfo[] touches;
}
