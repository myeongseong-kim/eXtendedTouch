using UnityEngine;
using TMPro;


public class HeadUpElement : MonoBehaviour
{
    [SerializeField]
    private Vector3 _eulerAngle;

    [SerializeField]
    private float _displacement;

    private Camera _mainCamera;


    void OnEnable()
    {
        _mainCamera = Camera.main;
    }

    void OnDisable()
    {

    }

    void LateUpdate()
    {
        this.transform.forward = _mainCamera.transform.forward;
        this.transform.localRotation *= Quaternion.Euler(_eulerAngle);
        this.transform.position = _mainCamera.transform.position;
        this.transform.localPosition += this.transform.forward * _displacement;
    }

}
