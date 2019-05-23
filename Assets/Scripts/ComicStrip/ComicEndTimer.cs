using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComicEndTimer : MonoBehaviour {

	[SerializeField] private String sceneName;
	
	// Use this for initialization
	void Start () {
		Invoke("StartScene", 2);
	}

	private void StartScene() {
		Application.LoadLevel(sceneName);
	}

}
