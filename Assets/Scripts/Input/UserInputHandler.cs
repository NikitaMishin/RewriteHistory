using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInputHandler : Singleton<UserInputHandler> {

	public delegate void KeyboardPress();

	public event KeyboardPress Escape;

	void Update() {
//		if (!Input.anyKey) return;
		
		if (Input.GetKeyUp(KeyCode.Escape)) {
			Escape?.Invoke();
			Debug.Log("Esc");
		}
		
//		Debug.Log(Input.);
	}

	protected override void Init() {
		DontDestroyOnLoad(this);
	}
}