using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ReverseTime;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    /*
     * USAGE: add for only one object
     * main controller for time rewinding, 
     * aka main for time logic, singleton
     * R - reverse only other objects without player
     * Q - reverse with other objects with player
     */

    public bool CouldUseReverse = true; // on some areas we can't use power
    public bool IsReversing = false; // all scripts listen to this field
    public float MaxTimeReverse = 5.0f; // max window for time rewind
    public bool IsUserShouldReverse = false; // weather user should also rewind


    private bool _shouldRemoveOldRecord = false; //is we need to delete old record  when reach MaxTimeReverse
    [SerializeField] private float _currentTimeReverse = 0f; //current window time

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.R) && CouldUseReverse)
        {
            _currentTimeReverse = Math.Max(_currentTimeReverse - Time.deltaTime, 0f);
            IsReversing = true;
            IsUserShouldReverse = false;
        }
        else if (Input.GetKey(KeyCode.Q) && CouldUseReverse)
        {
            _currentTimeReverse = Math.Max(_currentTimeReverse - Time.deltaTime, 0f);
            IsReversing = true;
            IsUserShouldReverse = true;
        }
        else
        {
            IsReversing = false;
            IsUserShouldReverse = false;
            _currentTimeReverse = Math.Min(MaxTimeReverse, _currentTimeReverse + Time.deltaTime);
        }

        _shouldRemoveOldRecord = _currentTimeReverse >= MaxTimeReverse;
    }

    public bool ShouldRemoveOldRecord()
    {
        return _shouldRemoveOldRecord;
    }
}