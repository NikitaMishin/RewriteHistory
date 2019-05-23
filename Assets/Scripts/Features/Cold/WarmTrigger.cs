using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarmTrigger : MonoBehaviour {

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
		
		_coldLogic.StayInWarm();
	}
}
