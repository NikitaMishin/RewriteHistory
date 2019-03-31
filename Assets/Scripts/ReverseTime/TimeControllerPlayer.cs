using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ReverseTime;
using UnityEngine;

public class TimeControllerPlayer : MonoBehaviour
{
    /*
     * USAGE: add for only one object
     * main controller for time rewinding, 
     * aka main for time logic, singleton
     * R - reverse only other objects without player
     * Q - reverse with other objects with player
     */
    [SerializeField] protected float currentTimeReverse = 0f; //current window time
    [SerializeField] protected float speed = 1f;
    public bool IsUserShouldReverse = false; // weather user should also rewind

    public bool CouldUseReverse = true; // on some areas we can't use power
    public bool IsReversing = false; // all scripts listen to this field
    public float MaxTimeReverse = 5.0f; // max window for time rewind

    protected bool shouldRemoveOldRecord = false; //is we need to delete old record  when reach MaxTimeReverse

    private bool start = false;
    private ManagerStates _managerStates;

    private void Awake()
    {
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) && CouldUseReverse)
        {
            currentTimeReverse = Math.Max(currentTimeReverse - Time.deltaTime, 0f);
            IsReversing = true;

            if (!start)
                _managerStates.canRewind = _managerStates.GetCurrentState().Equals(State.Dead);

            start = true;

            IsUserShouldReverse = _managerStates.canRewind;
        }
        else
        {
            start = false;
            _managerStates.canRewind = false;
            IsReversing = false;
            IsUserShouldReverse = false;
            currentTimeReverse = Math.Min(MaxTimeReverse, currentTimeReverse + Time.deltaTime);
        }

        shouldRemoveOldRecord = currentTimeReverse >= MaxTimeReverse;
    }

    public bool ShouldRemoveOldRecord()
    {
        return shouldRemoveOldRecord;
    }
}