using UnityEngine;


public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
	public static T Instance { get; private set; }

	protected virtual void Awake() {
		if (Instance == null) {
			Instance = (T) this;
			Init();
		}
		else
			Destroy(this);
	}

	protected virtual void Init() {
		
	}
}
