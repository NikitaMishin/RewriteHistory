using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuLoader : MonoBehaviour {
    [SerializeField] Scene _Scene;
    [SerializeField] string _SceneName;
    [SerializeField] int _SceneNumber = 0;

    void OnEnable() {
        UserInputHandler.Instance.Escape += OnEscape;
    }

    void OnDisable() {
        UserInputHandler.Instance.Escape -= OnEscape;
    }

    void OnEscape() {
        SceneLoader.Instance.LoadScene(_SceneNumber);
    }
}