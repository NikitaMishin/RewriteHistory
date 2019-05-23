﻿using System;
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
        if (!_checkPointController.SetTrigger(this, other.gameObject.transform.position, other.gameObject.transform.rotation))
            return;

        try
        {
            Messenger.Broadcast(GameEventTypes.CHECKPOINT);
        } catch (Exception e)
        {

        }
    }
}
