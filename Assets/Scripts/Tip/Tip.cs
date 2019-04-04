using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class Tip : MonoBehaviour {

    private TextMeshProUGUI _textMeshPro;

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
        _textMeshPro.text = text;
    }

    public void SetVisible(bool value)
    {
        gameObject.SetActive(value);
    }

    public bool GetVisible()
    {
        return gameObject.activeSelf;
    }
}
