using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdTrigger : MonoBehaviour
{
	[SerializeField] private float timeCold = 5f;
	
	private ColdLogic _coldLogic;

	// Use this for initialization
	void Start ()
	{
		_coldLogic = FindObjectOfType<ColdLogic>();
	}

	private void OnTriggerStay(Collider other)
	{
		if (!other.gameObject.tag.Equals("Player"))
			return;
		
		_coldLogic.StayInCold(timeCold);
	}
}
