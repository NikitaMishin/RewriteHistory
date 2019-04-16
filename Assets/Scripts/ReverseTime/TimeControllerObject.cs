using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerObject : TimeControllerPlayer {

    private ManagerStates _managerStates;

    private void Start()
    {
        _managerStates = FindObjectOfType<ManagerStates>();
    }

    private void FixedUpdate()
    {
        if (_managerStates.GetCurrentState() == State.Dead)
            return;

        if (Input.GetKey(KeyCode.Q) && CouldUseReverse)
        {
            currentTimeReverse = Mathf.Max(currentTimeReverse - Time.deltaTime, 0f);
            IsReversing = true;
        }
        else
        {
            IsReversing = false;
            currentTimeReverse = Mathf.Min(MaxTimeReverse, currentTimeReverse + Time.deltaTime);
        }

        shouldRemoveOldRecord = currentTimeReverse >= MaxTimeReverse;
    }
}
