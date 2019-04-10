using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour {

    private CheckPointController _checkPointController;

	// Use this for initialization
	void Start () {
        _checkPointController = FindObjectOfType<CheckPointController>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if (_checkPointController.GetTrigger() == this)
            return;

        Messenger.Broadcast(GameEventTypes.CHECKPOINT);
        _checkPointController.SetTrigger(this);
    }
}
