using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColdText : MonoBehaviour
{

	private Text _textMesh;
	private string _text = "Вы замерзли..." +
	                       "\nНажмите Q, чтобы попробовать снова";
	
	// Use this for initialization
	void Awake ()
	{
		_textMesh = GetComponent<Text>();
		
		Messenger.AddListener(GameEventTypes.DEAD, SetEnable);
		Messenger.AddListener(GameEventTypes.DEFAULT, SetDisable);
	}

	private void SetDisable()
	{
		_textMesh.text = string.Empty;
	}

	private void SetEnable()
	{
		_textMesh.text = _text;
	}
	
}
