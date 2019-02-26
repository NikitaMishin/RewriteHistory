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

    [SerializeField]
    private StartTrigger trigger;

    private LinkedList<BezierCurveObjectTimePoint> _timePoints;

    public BezierCurve Path;
    public int CurrentWayPointId = 0;
    public float Speed = 0.5f;
    public float RotationSpeed = 5f;
    public bool Direction = true;

    public float reachDistance = 1.0f; //to smooth

    private List<Vector3> PathPoints;

    private TimeController _timeController;

    // Use this for initialization
    void Start()
    {
        PathPoints = new List<Vector3>(Path.resolution * Path.pointCount);
        PathPoints.AddRange(Path.GetAllPointsAlongCurve());
        _timeController = FindObjectOfType<TimeController>();
        _timePoints = new LinkedList<BezierCurveObjectTimePoint>();
    }

    private void FixedUpdate()
    {
        if (ShouldRewind())
        {
            //start reverse
            StartRewind();
        }
        else
        {
            //record point
            RecordTimePoint();
        }

        if (_timeController.ShouldRemoveOldRecord())
        {
            DeleteOldRecord();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (ShouldRewind()) return;

        if (trigger != null && !trigger.WasStepped()) return;

        float distance = Vector3.Distance(PathPoints[CurrentWayPointId], transform.position);
        transform.position =
            Vector3.MoveTowards(transform.position, PathPoints[CurrentWayPointId], Time.deltaTime * Speed);

        var lookPos = PathPoints[CurrentWayPointId] - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);

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
        //closed path with positive direction and last point
        if (Direction && CurrentWayPointId >= PathPoints.Count - 1 && Path.close)
        {
            CurrentWayPointId = 0;
        }
        else if (Direction && CurrentWayPointId >= PathPoints.Count - 1 && !Path.close)
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
            new BezierCurveObjectTimePoint(transform.position, transform.rotation, Direction, CurrentWayPointId));
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
}