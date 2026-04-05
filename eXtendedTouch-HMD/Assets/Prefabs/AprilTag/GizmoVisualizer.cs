using UnityEngine;
using System;


[ExecuteAlways]
public class GizmoVisualizer : MonoBehaviour
{
    private Material _lineMaterial;

    void Start()
    {
        _lineMaterial = new Material(Shader.Find("Hidden/Internal-Colored"));
        _lineMaterial.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        _lineMaterial.SetInt("_ZWrite", 0);
        _lineMaterial.renderQueue = 4000; 
    }

    void Update()
    {

    }

    void OnDrawGizmos() 
    {
        var originalMatrix = Gizmos.matrix;
        Gizmos.matrix = this.transform.localToWorldMatrix;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(Vector3.zero, 0.5f * Vector3.right);
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Vector3.zero, 0.5f * Vector3.up);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(Vector3.zero, 0.5f * Vector3.forward);

        Gizmos.matrix = originalMatrix;
    }

    void OnRenderObject() 
    {
        if (_lineMaterial == null)
            return;

        GL.PushMatrix();
        GL.MultMatrix(transform.localToWorldMatrix);

        _lineMaterial.SetPass(0);

        GL.Begin(GL.LINES);

        GL.Color(Color.red);
        GL.Vertex3(0, 0, 0); 
        GL.Vertex3(0.5f, 0, 0); 

        GL.Color(Color.green);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0.5f, 0);

        GL.Color(Color.blue);
        GL.Vertex3(0, 0, 0);
        GL.Vertex3(0, 0, 0.5f);

        GL.End();

        GL.PopMatrix();
    }
}