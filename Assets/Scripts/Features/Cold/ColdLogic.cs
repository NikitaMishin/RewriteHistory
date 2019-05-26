using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdLogic : MonoBehaviour
{

	[SerializeField] private float warmTime = 5f;
	
	public ColdAlphaController _alpha;
	private float _timeForTickWarm;
	private float _timeForTickCold;
	
	private float _startTime;
	private ManagerStates _managerStates;

	// Use this for initialization
	void Start ()
	{
		_alpha = FindObjectOfType<ColdAlphaController>();
		_managerStates = FindObjectOfType<ManagerStates>();
		_timeForTickWarm = warmTime / 100f;
		
		_startTime = Time.time;
		Messenger.AddListener(GameEventTypes.DEFAULT, Restart);
	}

	public void StayInCold(float time)
	{
		_timeForTickCold = time / 100f;
		
		if (IsTimeForTickCold())
		{
			_alpha.IncreaseAlpha();
			_startTime = Time.time;
		}

		if (_alpha.GetAlpha() > 0.9f)
		{
			_managerStates.ChangeState(State.Dead);
		}
	}

	private bool IsTimeForTickCold()
	{
		return Time.time - _startTime > _timeForTickCold;
	}

	public void StayInWarm()
	{
		if (IsTimeForTickWarm())
		{
			_alpha.DecreaseAlpha();
			_startTime = Time.time;
			
		}
	}
	
	private bool IsTimeForTickWarm()
	{
		return Time.time - _startTime > _timeForTickWarm;
	}
	
	private void Restart()
	{
		_alpha.SetAlpha(0);
	}
}
