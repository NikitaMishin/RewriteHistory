using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComicController : MonoBehaviour
{
	[SerializeField] private GameObject[] panels;
	[SerializeField] private string nextScene;
	[SerializeField] private Animator animator;
	private Button button;
	
	private int currentPanel = 0;

	private void Start()
	{
		button = GetComponent<Button>();
	}

	public void OnButtonClick()
	{
		if (currentPanel < panels.Length - 1)
		{
			button.interactable = false;
			currentPanel++;
			panels[currentPanel].SetActive(true);
			animator.ResetTrigger("Enable");
			animator.SetTrigger("Disable");
		}
		else
		{
			Application.LoadLevel(nextScene);
		}
	}
}
