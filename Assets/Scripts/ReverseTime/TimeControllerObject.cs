using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerObject : TimeControllerPlayer {

    private ManagerStates _managerStates;
    private ManagerController _managerController;

    private void Start()
    {
        _managerStates = FindObjectOfType<ManagerStates>();
        _managerController = FindObjectOfType<ManagerController>();
        
        Messenger.AddListener(GameEventTypes.DEFAULT, RestoreTimeUpdate);
    }

    private void RestoreTimeUpdate()
    {
        currentTimeReverse = 0;
    }

    private void FixedUpdate()
    {
        if (_managerStates.GetCurrentState() == State.Dead)
            return;

        if (Input.GetKey(KeyCode.Q) && CouldUseReverse && _managerController.CanRewind())
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
