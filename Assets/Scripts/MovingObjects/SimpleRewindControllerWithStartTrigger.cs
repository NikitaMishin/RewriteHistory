using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRewindControllerWithStartTrigger : MonoBehaviour, IRevertListener {

    /*
	 * USAGE:
	 * Just add this script to simple object(not bezier Curve!!) that will be reversed when user press rewind 
	 * Take no assumption about what object(store only position and rotation)
	 * space complexity = fixedUpdatePerSec * TimeWindow * 7float
	 * 820kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte
	 */
    // Use this for initialization
    [SerializeField] private StartTrigger startTrigger;

    private LinkedList<TimePointWithStartTrigger> _timePoints;
    private TimeControllerObject _timeController;

    void Start()
    {
        _timePoints = new LinkedList<TimePointWithStartTrigger>();
        _timeController = FindObjectOfType<TimeControllerObject>();
    }

    // Update is called once per frame
    void Update()
    {

        if (ShouldRewind())
        {
            StartRewind();
        }
        else
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
        _timePoints.AddLast(new TimePointWithStartTrigger(transform.position, transform.rotation, startTrigger.WasStepped()));
    }

    public void StartRewind()
    {
        if (_timePoints.Count > 0)
        {
            var timePoint = _timePoints.Last.Value;
            transform.position = timePoint.position;
            transform.rotation = timePoint.rotation;
            startTrigger.SetWasStepped(timePoint.wasStepped);
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
        throw new System.NotImplementedException();
    }

    public bool ShouldRewind()
    {
        return _timeController.IsReversing;
    }
}
