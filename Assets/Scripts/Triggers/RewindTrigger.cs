using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTrigger : MonoBehaviour
{
	private ManagerController _managerController;

	private void Start()
	{
		_managerController = FindObjectOfType<ManagerController>();
	}

	private void OnTriggerEnter(Collider other)
	{
		_managerController.SetRewind(true);
	}
}
