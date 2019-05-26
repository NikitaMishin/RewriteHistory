using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class Tip : MonoBehaviour {

    private TextMeshProUGUI _textMeshPro;

    [SerializeField] private Image image;
	// Use this for initialization
	void Awake () {
        _textMeshPro = gameObject.GetComponentInChildren<TextMeshProUGUI>();
	}
	
    public string GetText()
    {
        return _textMeshPro.text;
    }

    public void SetText(string text)
    {
        _textMeshPro.gameObject.SetActive(true);
        image.gameObject.SetActive(false);
        _textMeshPro.text = text;
    }

    public void SetVisible(bool value)
    {
        transform.GetChild(0).gameObject.SetActive(value);
    }
    
    public void SetImage(Sprite sprite)
    {
        _textMeshPro.gameObject.SetActive(false);
        image.gameObject.SetActive(true);
        image.sprite = sprite;
    }
    
}
