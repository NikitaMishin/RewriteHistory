using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineActivator : MonoBehaviour {
    [SerializeField] PlayableDirector director;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        director.RebuildGraph();
        director.Play();
    }
}
