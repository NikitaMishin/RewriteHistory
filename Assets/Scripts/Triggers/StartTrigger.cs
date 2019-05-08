using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour {

    [SerializeField]
    private BezierCurveMovementWithRewind bezierCurveMovement;
    [SerializeField]
    private bool hasFewTriggers = false;

    private bool _wasStepped = false;
    private bool _wasCLosed = false;

    private void Start()
    {
        if (hasFewTriggers)
            bezierCurveMovement.SetFewTrigger(true);
    }

    private void OnTriggerStay(Collider other)
    {
        _wasStepped = true;
        _wasCLosed = false;

        if (hasFewTriggers)
        {
            bezierCurveMovement.wasStepped = true;
            bezierCurveMovement.SetWasClosed(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _wasCLosed = true;

        if (hasFewTriggers)
        {
            bezierCurveMovement.SetWasClosed(true);
        }
    }

    public bool WasStepped()
    {
        return _wasStepped;
    }

    public void SetWasStepped(bool value)
    {
        _wasStepped = value;
    }

    public bool WasClosed()
    {
        return _wasCLosed;
    }

    public void SetWasClosed(bool value)
    {
        _wasCLosed = value;
    }

    public bool HasFewTriggers()
    {
        return hasFewTriggers;
    }
}
