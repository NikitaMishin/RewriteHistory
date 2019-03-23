﻿using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ReverseTime;

public class FallenColumnRewind : MonoBehaviour, IRevertListener {

    /*
	 * USAGE:
	 * Just add this script to  object with rigidBody that will be reversed when user press rewind 
	 * Take  assumption that object have rigidBody
	 *  space complexity = fixedUpdatePerSec * TimeWindow * 13float
	 * 1523kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte
	 */
    // Use this for initialization
    [SerializeField] private FallenColumn fallenColumn;

	private LinkedList<RigidBodyFallenColumnTimePoint> _timePoints;
    private TimeController _timeController;
    private Rigidbody _rb;
    private Collider _collider;

    void Start()
    {
        _timePoints = new LinkedList<RigidBodyFallenColumnTimePoint>();
        _timeController = FindObjectOfType<TimeController>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
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
            _collider.enabled = true;
            RecordTimePoint();
        }

        if (_timeController.ShouldRemoveOldRecord())
        {
            DeleteOldRecord();
        }


    }

    public void RecordTimePoint()
    {
        _timePoints.AddLast(new RigidBodyFallenColumnTimePoint(transform.position, transform.rotation, _rb.velocity, _rb.angularVelocity, fallenColumn.WasStepped()));
    }

    public void StartRewind()
    {
        if (_timePoints.Count > 0)
        {
            var timePoint = _timePoints.Last.Value;
            transform.position = timePoint.Position;
            transform.rotation = timePoint.Rotation;
            _rb.velocity = timePoint.Velocity;
            _rb.angularVelocity = timePoint.AngularVelocity;
            _timePoints.RemoveLast();
            _collider.enabled = false;
            //  _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
            _rb.velocity = Vector3.zero;

            fallenColumn.SetWasStepped(false);

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
