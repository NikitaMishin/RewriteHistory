using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdLogic : MonoBehaviour
{

	[SerializeField] private float time = 5f;
	
	private ColdAlphaController _alpha;
	private float _timeForTick;

	private float _startTime;
	private ManagerStates _managerStates;
	
	
	// Use this for initialization
	void Start ()
	{
		_alpha = FindObjectOfType<ColdAlphaController>();
		_managerStates = FindObjectOfType<ManagerStates>();
		_timeForTick = time / 100;
		
		_startTime = Time.time;
		Messenger.AddListener(GameEventTypes.DEFAULT, Restart);
	}

	public void StayInCold()
	{
		if (IsTimeForTick())
		{
			_alpha.IncreaseAlpha();
			_startTime = Time.time;
			
		}

		if (_alpha.GetAlpha() > 0.9f)
		{
			_managerStates.ChangeState(State.Dead);
		}
	}

	public void StayInWarm()
	{
		if (IsTimeForTick())
		{
			_alpha.DecreaseAlpha();
			_startTime = Time.time;
			
		}
	}
	
	private bool IsTimeForTick()
	{
		return Time.time - _startTime > _timeForTick;
	}
	
	private void Restart()
	{
		_alpha.SetAlpha(0);
	}
}
