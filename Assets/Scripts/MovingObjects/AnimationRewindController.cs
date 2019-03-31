using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRewindController : MonoBehaviour, IRevertListener
{

    /*
	 * USAGE:
	 * Just add this script to  object with rigidBody that will be reversed when user press rewind 
	 * Take  assumption that object have rigidBody
	 *  space complexity = fixedUpdatePerSec * TimeWindow * 13float
	 * 1523kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte
	 */
    // Use this for initialization

    [SerializeField]
    private AnimationRewind animationRewind;

    private LinkedList<AnimationTimePoint> _timePoints;
    private TimeControllerPlayer _timeController;

    void Start()
    {
        _timePoints = new LinkedList<AnimationTimePoint>();
        _timeController = FindObjectOfType<TimeControllerPlayer>();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        _timePoints.AddLast(new AnimationTimePoint(animationRewind.GetTime(), animationRewind.GetSpeed()));
    }

    public void StartRewind()
    {
        if (_timePoints.Count > 0)
        {
            var timePoint = _timePoints.Last.Value;
            animationRewind.SetTime(timePoint.currentTime);
            animationRewind.SetSpeed(timePoint.speed);
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
