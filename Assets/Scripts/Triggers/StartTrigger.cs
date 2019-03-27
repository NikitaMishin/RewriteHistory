using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour {

    private bool _wasStepped = false;
    private bool _wasCLosed = false;

    private void OnTriggerStay(Collider other)
    {
        _wasStepped = true;
        _wasCLosed = false;
    }

    private void OnTriggerExit(Collider other)
    {
        _wasCLosed = true;
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
}
