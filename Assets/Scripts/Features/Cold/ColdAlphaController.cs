using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColdAlphaController : MonoBehaviour
{
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
    }

    public void IncreaseAlpha()
    {
        Color color = _image.color;
        color.a += 0.01f;
        _image.color = color;
    }

    public void DecreaseAlpha()
    {
        Color color = _image.color;
        color.a -= 0.01f;
        _image.color = color;
    }

    public float GetAlpha()
    {
        return _image.color.a;
    }

    public void SetAlpha(float value)
    {
        Color color = _image.color;
        color.a = value;
        _image.color = color;
    }
}
