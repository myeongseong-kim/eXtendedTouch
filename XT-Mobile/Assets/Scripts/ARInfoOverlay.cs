using UnityEngine;
using UnityEngine.XR.ARFoundation;
using TMPro;


public class ARInfoOverlay : MonoBehaviour
{
    [Header("Refs")]
    public ARSession _arSession;
    public Camera _arCamera;
    public TMP_Text _arText;

    void Update()
    {
        if (_arCamera == null) return;

        Vector3 pos = _arCamera.transform.position;
        Quaternion rot = _arCamera.transform.rotation;
        // Vector3 euler = rot.eulerAngles;

        string session = (_arSession != null) ? ARSession.state.ToString() : "N/A";

        _arText.text = 
            $"State: {session}\n" + 
            $"Position: {pos.x:F3}, {pos.y:F3}, {pos.z:F3}\n" +
            $"Rotation: {rot.x:F3}, {rot.y:F3}, {rot.z:F3}, {rot.w:F3}\n";

        // _arText.text =
        //     $"State: {session}\n" +
        //     $"Position: {pos.x:F3}, {pos.y:F3}, {pos.z:F3}\n" +
        //     $"Rotation: {euler.x:F1}, {euler.y:F1}, {euler.z:F1}\n";
    }
}
