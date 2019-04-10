using System.Collections;
using System.Collections.Generic;
using System.Xml;
using ReverseTime;
using UnityEngine;

public class SimpleRewind : MonoBehaviour,IRevertListener {

	/*
	 * USAGE:
	 * Just add this script to simple object(not bezier Curve!!) that will be reversed when user press rewind 
	 * Take no assumption about what object(store only position and rotation)
	 * space complexity = fixedUpdatePerSec * TimeWindow * 7float
	 * 820kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte
	 */
	// Use this for initialization
	private LinkedList<SimpleTimePoint> _timePoints;
    private SimpleTimePoint _checkPoint;
	private TimeControllerPlayer _timeController;

    private ManagerStates _managerStates;
	
	void Start ()
	{
		_timePoints =  new LinkedList<SimpleTimePoint>();
		_timeController = FindObjectOfType<TimeControllerPlayer>();
        _managerStates = FindObjectOfType<ManagerStates>();
        Messenger.AddListener(GameEventTypes.CHECKPOINT, SavePosition);
        Messenger.AddListener(GameEventTypes.DEAD, RestartPosition);
    }

    private void SavePosition()
    {
        _checkPoint = new SimpleTimePoint(transform.position, transform.rotation);
    }

    private void RestartPosition()
    {
        transform.position = _checkPoint.position;
        transform.rotation = _checkPoint.rotation;
    }

    // Update is called once per frame
    void Update () {

		if (ShouldRewind())
		{
			StartRewind();
		}
		else if (_managerStates.GetCurrentState() != State.Dead)
        {
			RecordTimePoint();
		}

		if (_timeController.ShouldRemoveOldRecord())
		{
			DeleteOldRecord();
		}
		
		
	}

	public void RecordTimePoint()
	{
		_timePoints.AddLast( new SimpleTimePoint(transform.position, transform.rotation));
	}

	public void StartRewind()
	{
		if (_timePoints.Count > 0)
		{
			var timePoint = _timePoints.Last.Value;
			transform.position = timePoint.position;
			transform.rotation = timePoint.rotation;
			_timePoints.RemoveLast();
		}
	}

	public void DeleteOldRecord()
	{
		if (_timePoints.Count > 0)
		{
			_timePoints.RemoveFirst();
		}
	}

	public void DeleteAllRecord()
	{
        _timePoints.Clear();
	}

	public bool ShouldRewind()
	{
		return _timeController.IsReversing;
	}
}
