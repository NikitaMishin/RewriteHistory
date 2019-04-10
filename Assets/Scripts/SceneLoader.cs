using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : Singleton<SceneLoader> {

    protected override void Init() {
        DontDestroyOnLoad(this);
    }
    
    public void LoadScene(int sceneNumber) {
        StartCoroutine(AsyncSceneLoad(sceneNumber));
    }

    public void LoadScene(string sceneName) {
        StartCoroutine(AsyncSceneLoad(sceneName));
    }

    static IEnumerator AsyncSceneLoad(int sceneNumber) {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneNumber);

        while (!asyncOperation.isDone) yield return null;
    }

    static IEnumerator AsyncSceneLoad(string sceneName) {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncOperation.isDone) yield return null;
    }


}