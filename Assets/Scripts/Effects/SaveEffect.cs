using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveEffect : MonoBehaviour
{
	private Image image;
	
	// Use this for initialization
	void Start ()
	{
		image = GetComponent<Image>();
		image.enabled = false;
		Messenger.AddListener(GameEventTypes.CHECKPOINT, Activate);
	}

	void Activate()
	{
		image.enabled = true;
		Invoke("Disable", 1);
	}

	void Disable()
	{
		image.enabled = false;
	}
}
