using UnityEngine;
using UnityEngine.UI;
using System;


public class TagSource : MonoBehaviour
{
    public enum TagType
    {
        TagStandard41h12
    }

    [SerializeField] private RawImage _tagImage;
    [SerializeField] private Texture2D _tagTexture;
    [SerializeField] private float _tagSize = 10.0f; // Size in millimeters
    [SerializeField] private TagType _tagType = TagType.TagStandard41h12;

    private Vector2 _resolution;
    private int _dpi;


    void Start()
    {
        _tagImage.texture = _tagTexture;

        _resolution = new Vector2(Screen.width, Screen.height);
        _dpi = Screen.dpi > 0 ? (int)Screen.dpi : 72; // Default to 72 DPI if unknown

        float size = _tagSize * _dpi / 25.4f;
        if (_tagType == TagType.TagStandard41h12)
        {
            size = size * (9f / 5f); // Adjust for the aspect ratio of the tag
        }

        _tagImage.rectTransform.sizeDelta = new Vector2(size, size);
    }


    void Update()
    {

    }
}
