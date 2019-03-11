using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineActivator : MonoBehaviour {
    [SerializeField] PlayableDirector director;
    [SerializeField] bool HowToDebug = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        director.Play();
        HowToDebug = true;

    }
}
