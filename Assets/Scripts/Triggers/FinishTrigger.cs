using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] private string sceneName = "ComicStripFinish";
    [SerializeField] private Image panel;

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(FinishAnimation());
    }

    IEnumerator FinishAnimation()
    {
        while (panel.color.a < 1.0f)
        {
            Color c = panel.color;
            c.a += 0.01f;
            panel.color = c;
            
            yield return new WaitForSecondsRealtime(0.01f);
        }
        
        SceneManager.LoadScene(sceneName);

    }
}
