using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTrigger : MonoBehaviour {

    private bool _wasStepped = false;

    private void OnTriggerStay(Collider other)
    {
        _wasStepped = true;
    }

    public bool WasStepped()
    {
        return _wasStepped;
    }

    public void SetWasStepped(bool value)
    {
        _wasStepped = value;
    }
}
