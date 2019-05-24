using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnableButton : MonoBehaviour
{

	[SerializeField] private Animator animator;
	[SerializeField] private Button button;

	private void Start()
	{
		button = FindObjectOfType<Button>();
	}

	public void EnableButtonNext()
	{
		button.interactable = true;
    	animator.SetTrigger("Enable");
    	animator.ResetTrigger("Disable");
	}
}
