using UnityEngine;
using System;
using System.Collections.Generic;
using Meta.XR;
using Oculus.Interaction.Input;
using AprilTag;


public class TagTracker : MonoBehaviour
{
    [SerializeField] private PassthroughCameraAccess _pca;
    private TagDetector _detector;

    private Dictionary<int, GameObject> _tags;
    [SerializeField] private GameObject _tagPrefab;
    [SerializeField] private float _tagSize;


    void Start()
    {
        var dims = _pca.CurrentResolution;
        _detector = new TagDetector(dims.x, dims.y, 2);
        _tags = new Dictionary<int, GameObject>();
    }

    void OnDestroy()
    {
        _detector.Dispose();
        foreach (var obj in _tags.Values)
        {
            Destroy(obj);
        }
    }


    void LateUpdate()
    {
        if (!_pca.IsPlaying)
        {
            return;
        }

        var image = _pca.GetColors();
        var pose = _pca.GetCameraPose();
        var intrinsics = _pca.Intrinsics;

        float w = _pca.CurrentResolution.x;
        float h = _pca.CurrentResolution.y;

        float fx, fy, cx, cy;
        GetImageSpaceIntrinsics(_pca, out fx, out fy, out cx, out cy);

        _detector.ProcessImage(image, fx, fy, cx, cy, _tagSize);

        Debug.Log($"Current={_pca.CurrentResolution} " +
            $"Sensor={intrinsics.SensorResolution} " +
            $"f=({fx:F2},{fy:F2}) c=({cx:F2},{cy:F2}) " +
            $"pp=({intrinsics.PrincipalPoint.x:F2},{intrinsics.PrincipalPoint.y:F2})");

        HashSet<int> detectedTagIds = new HashSet<int>();
        foreach (var tag in _detector.DetectedTags)
        {
            detectedTagIds.Add(tag.ID);

            var viewTransformMatrix = Matrix4x4.TRS(tag.Position, tag.Rotation, Vector3.one);
            var viewToWorldMatrix = Matrix4x4.TRS(pose.position, pose.rotation, Vector3.one);

            var worldTransformMatrix = viewToWorldMatrix * viewTransformMatrix;
            Vector3 worldPosition = worldTransformMatrix.GetPosition();
            Quaternion worldRotation = worldTransformMatrix.rotation;

            if (!_tags.ContainsKey(tag.ID))
            {
                var obj = Instantiate(_tagPrefab);
                obj.GetComponent<Tag>().SetId(tag.ID);
                obj.name = $"Tag#{tag.ID}";
                obj.transform.localScale = Vector3.one * _tagSize;
                obj.transform.position = worldPosition;
                obj.transform.rotation = worldRotation;
                _tags.Add(tag.ID, obj);
            }
            else
            {
                var obj = _tags[tag.ID];
                obj.transform.position = worldPosition;
                obj.transform.rotation = worldRotation;
            }
        }

        var ids = new HashSet<int>(_tags.Keys);
        foreach (var id in ids)
        {
            if (!detectedTagIds.Contains(id))
            {
                Destroy(_tags[id]);
                _tags.Remove(id);
            }
        }
    }


    private static void GetImageSpaceIntrinsics(
        PassthroughCameraAccess pca,
        out float fx, out float fy, out float cx, out float cy)
    {
        var intrinsics = pca.Intrinsics;

        Vector2 currentRes = (Vector2)pca.CurrentResolution;
        Vector2 sensorRes = intrinsics.SensorResolution;

        Vector2 scale = currentRes / sensorRes;
        scale /= Mathf.Max(scale.x, scale.y);

        Rect crop = new Rect(
            sensorRes.x * (1f - scale.x) * 0.5f,
            sensorRes.y * (1f - scale.y) * 0.5f,
            sensorRes.x * scale.x,
            sensorRes.y * scale.y
        );

        float sx = currentRes.x / crop.width;
        float sy = currentRes.y / crop.height;

        fx = intrinsics.FocalLength.x * sx;
        fy = intrinsics.FocalLength.y * sy;
        cx = (intrinsics.PrincipalPoint.x - crop.x) * sx;
        cy = (intrinsics.PrincipalPoint.y - crop.y) * sy;
    }
}
