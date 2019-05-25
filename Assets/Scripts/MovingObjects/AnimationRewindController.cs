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

    private ManagerStates _managerStates;
    private LinkedList<AnimationTimePoint> _timePoints;
    private AnimationTimePoint _checkPoint;
    private TimeControllerObject _timeControllerObject;

    void Start()
    {
        _managerStates = FindObjectOfType<ManagerStates>();
        _timePoints = new LinkedList<AnimationTimePoint>();
        _timeControllerObject = FindObjectOfType<TimeControllerObject>();
        Messenger.AddListener(GameEventTypes.CHECKPOINT, SavePosition);
        Messenger.AddListener(GameEventTypes.DEFAULT, RestartPosition);
    }

    private void SavePosition()
    {
        StartCoroutine(Save());
    }
    
    IEnumerator Save()
    {
        _checkPoint = new AnimationTimePoint(animationRewind.GetTime(), animationRewind.GetSpeed());

        yield return null;
    } 
    
    IEnumerator Restart()
    {
        animationRewind.SetTime(_checkPoint.currentTime);
        animationRewind.SetSpeed(_checkPoint.speed);
        
        yield return null;
    } 

    private void RestartPosition()
    {
        StartCoroutine(Restart());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (ShouldRewind())
        {
            StartRewind();
        }
        else if (_managerStates.GetCurrentState() != State.Dead)
        {
            RecordTimePoint();
        }

        if (_timeControllerObject.ShouldRemoveOldRecord())
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
        return _timeControllerObject.IsReversing;
    }
}
