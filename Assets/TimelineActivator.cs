using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineActivator : MonoBehaviour {
    [SerializeField] PlayableDirector director;

    private ManagerController _managerController;

    private void Start()
    {
        _managerController = FindObjectOfType<ManagerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (_managerController.playableDirector == director)
            return;

        director.RebuildGraph();
        director.Play();
        _managerController.playableDirector = director;

    }
}
