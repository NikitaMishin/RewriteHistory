using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class ColdActivation : MonoBehaviour
{
	
	[SerializeField] private bool isEnd;
	
	private ColdText coldText;

	void Start ()
	{
		coldText = FindObjectOfType<ColdText>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (isEnd)
			coldText.transform.parent.gameObject.SetActive(false);
		else
			coldText.transform.parent.gameObject.SetActive(true);
	}
}
