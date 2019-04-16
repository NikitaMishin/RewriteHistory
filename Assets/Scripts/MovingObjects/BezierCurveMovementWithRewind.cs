using System;
using System.Collections;
using System.Collections.Generic;
using ReverseTime;
using UnityEngine;

public class BezierCurveMovementWithRewind : MonoBehaviour, IRevertListener
{
    /*
   * NOTE: direction of Curve determined by direction of point (0,1,2,3 and so on) e.t 01234==positive,4321=negative
     * COULD REWIND,start rewind by pressing button that defined in TimeController
   * USAGE:
   * Add script to object that must move along curved path
   * Add bezierPath to this script
     * space complexity = fixedUpdatePerSec * TimeWindow * (7 float+int + bool)
	 * 1523kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte,int4
   */

    [SerializeField] private StartTrigger trigger;
    [SerializeField] private bool withExitTrigger;
    [SerializeField] private float timeAfterExit = 2f;

    private bool _wasExecuteBack = false;

    private LinkedList<BezierCurveObjectTimePoint> _timePoints;

    private BezierCurveObjectTimePoint _checkPoint;

    private float _timeClosedStart = 0;

    public BezierCurve Path;
    public int CurrentWayPointId = 0;
    public float Speed = 0.5f;
    public float RotationSpeed = 5f;
    public bool Direction = true;
    public bool isShouldStopAtTheEnd = false;
    public GameObject objectWithActivatedTrigger = null;
    public bool wasStepped = false;
    public bool wasClosed = false;

    private ManagerStates _managerStates;

    public float reachDistance = 1.0f; //to smooth

    private List<Vector3> PathPoints;

    private TimeControllerPlayer _timeController;

    // Use this for initialization
    void Start()
    {
        PathPoints = new List<Vector3>(Path.resolution * Path.pointCount);
        PathPoints.AddRange(Path.GetAllPointsAlongCurve());
        _managerStates = FindObjectOfType<ManagerStates>();
      
        _timeController = FindObjectOfType<TimeControllerObject>();

        _timePoints = new LinkedList<BezierCurveObjectTimePoint>();
        if ( trigger == null && objectWithActivatedTrigger != null)
        { //trigger == null in case if we set manually this
            trigger = objectWithActivatedTrigger.GetComponent<StartTrigger>();
        }

        if (!Direction)
        {
            CurrentWayPointId = PathPoints.Count - 1;
        }

        Messenger.AddListener(GameEventTypes.CHECKPOINT, SavePosition);
        Messenger.AddListener(GameEventTypes.DEAD, RestartPosition);

    }

    private void SavePosition()
    {
        _checkPoint = new BezierCurveObjectTimePoint(
                transform.position,
                transform.rotation,
                Direction,
                CurrentWayPointId,
                trigger == null ? false : trigger.HasFewTriggers() ? trigger.WasStepped() : wasStepped
                );
    }

    private void RestartPosition()
    {
        transform.position = _checkPoint.position;
        transform.rotation = _checkPoint.rotation;
        CurrentWayPointId = _checkPoint.curWaypointIndex;
        Direction = _checkPoint.curveDir;

        if (trigger != null)
        {
            wasStepped = _checkPoint.wasStepped;
            trigger.SetWasStepped(_checkPoint.wasStepped);
        }

        DeleteAllRecord();
    }

    private void FixedUpdate()
    {
        if (ShouldRewind())
        {
            //start reverse
            StartRewind();
        }
        else if (_managerStates.GetCurrentState() != State.Dead)
        {
            //record point
            RecordTimePoint();
        }

        if (_timeController.ShouldRemoveOldRecord())
        {
            DeleteOldRecord();
        }
    }

    public bool IsReachedEndOfCurve()
    {
        return ((CurrentWayPointId == PathPoints.Count - 1) && Direction) || ((CurrentWayPointId == 0) && !Direction);
    }


    // Update is called once per frame
    void Update()
    {
        if (ShouldRewind()) return;// || _managerStates.GetCurrentState() == State.Dead) return;
        if (trigger != null && !trigger.WasStepped()) return;
        if (isShouldStopAtTheEnd && IsReachedEndOfCurve() && !withExitTrigger) return;
        if (trigger != null && withExitTrigger && !wasClosed && !wasStepped) return;

        float distance = Vector3.Distance(PathPoints[CurrentWayPointId], transform.position);
        transform.position =
            Vector3.MoveTowards(transform.position, PathPoints[CurrentWayPointId], Time.deltaTime * Speed);


        var lookPos = PathPoints[CurrentWayPointId] - transform.position;


        Quaternion rotation = Quaternion.Euler(0, 0, 0);

        if (lookPos != Vector3.zero)
        {
            rotation = Quaternion.LookRotation(lookPos);
        }

        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);

        if (distance <= reachDistance)
        {
            UpdateIndexPoint();
        }
    }


    /// <summary>
    /// Assign next point to follow based on  Direction Current point and Path.close
    /// </summary>
    private void UpdateIndexPoint()
    {
        if (withExitTrigger)
            UpdatePointWithExitTriggerNext();
        else
            UpdatePointWithoutExitTrigger();
        
    }

    private void UpdatePointWithExitTriggerNext()
    {
        if (!Direction && CurrentWayPointId == 0 && !Path.close)
        {
            wasStepped = false;
            wasClosed = false;
            CurrentWayPointId = 0;
            Direction = !Direction;
            _wasExecuteBack = false;
        }
        else if (wasStepped && !wasClosed && CurrentWayPointId < PathPoints.Count - 1)
        {
            CurrentWayPointId++;
            _wasExecuteBack = false;
        }
        else if (wasClosed && Time.time - _timeClosedStart > timeAfterExit)
        {
            UpdatePointWithExiteBack();
        }
    }

    private void UpdatePointWithExiteBack()
    {
   //     if (wasStepped)
     //       return;

        if (wasClosed && Direction && !Path.close && CurrentWayPointId > 0)
        {
            CurrentWayPointId--;
            Direction = !Direction;
        }
        else if (wasClosed && !Direction && CurrentWayPointId > 0)
        {
            CurrentWayPointId--;
        }
        else if (!wasClosed && !Direction)
        {
            Direction = !Direction;
        }
    }

    private void UpdatePointWithoutExitTrigger()
    {
        if (Direction && CurrentWayPointId >= PathPoints.Count - 1 && Path.close)
        {
            CurrentWayPointId = 0;
        }
        else if (CurrentWayPointId >= PathPoints.Count - 1 && !Path.close)
        {
            CurrentWayPointId--;
            Direction = !Direction;
        }
        else if (!Direction && CurrentWayPointId == 0 && !Path.close)
        {
            CurrentWayPointId++;
            Direction = !Direction;
        }
        else if (Direction)
        {
            CurrentWayPointId++;
        }
        else if (!Direction)
        {
            CurrentWayPointId--;
        }
    }
    


    /// <summary>
    /// Record current position with transformation
    /// </summary>
    public void RecordTimePoint()
    {
        _timePoints.AddLast(
            new BezierCurveObjectTimePoint(
                transform.position,
                transform.rotation,
                Direction,
                CurrentWayPointId,
                trigger == null ? false : trigger.HasFewTriggers() ? trigger.WasStepped() : wasStepped
                )
            );
    }

    /// <summary>
    /// Start rewind time and remove  last point
    /// </summary>
    public void StartRewind()
    {
        if (_timePoints.Count == 0)
        {
            return;
        }

        var tmp = _timePoints.Last.Value;
        //apply

        transform.position = tmp.position;
        transform.rotation = tmp.rotation;
        CurrentWayPointId = tmp.curWaypointIndex;
        Direction = tmp.curveDir;

        if (trigger != null)
        {
            wasStepped = tmp.wasStepped;
            trigger.SetWasStepped(tmp.wasStepped);
        }

        //remove record
        _timePoints.RemoveLast();
    }

    /// <summary>
    /// Delete Old(zero) record
    /// </summary>
    public void DeleteOldRecord()
    {
        if (_timePoints.Count > 0)
        {
            _timePoints.RemoveFirst();
            //     Debug.Log(_timePoints.Count);
        }
    }

    /// <summary>
    /// Delete All records
    /// </summary>
    public void DeleteAllRecord()
    {
        _timePoints.Clear();
    }

    public bool ShouldRewind()
    {
        return _timeController.IsReversing;
    }

    internal void SetWasClosed(bool v)
    {
        wasClosed = v;
        _timeClosedStart = Time.time;
    }
}