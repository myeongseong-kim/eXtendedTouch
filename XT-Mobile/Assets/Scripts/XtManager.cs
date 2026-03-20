using UnityEngine;

public class XtManager : MonoBehaviour
{
    [SerializeField] private GameObject _indicatorObject;


    void Start()
    {
        
    }


    void Update()
    {
        
    }


    public void SetTransform(Vector3 position, Quaternion rotation)
    {
        _indicatorObject.transform.position = position;
        _indicatorObject.transform.rotation = rotation;
    }

    public void SetScreenSize(float width, float height)
    {
        _indicatorObject.transform.localScale = new Vector3(width, height, 0.01f);
    }
}
