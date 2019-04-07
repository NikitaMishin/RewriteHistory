using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAfterTrigger : MonoBehaviour {
    [SerializeField] private StartTrigger startTrigger;
    [SerializeField] private Vector3 angleAfterTrigger;
    [SerializeField] private float speed = 5;

    private float _speed;
    private Quaternion _angle;
    private ManagerStates _managerStates;

    private void Start()
    {
        _speed = speed;
        _angle = Quaternion.Euler(angleAfterTrigger);
        _managerStates = FindObjectOfType<ManagerStates>();
    }

    // Update is called once per frame
    void Update () {
        if (_managerStates.GetCurrentState() == State.Dead)
            return;

        if (!startTrigger.WasStepped())
        {
            speed = _speed;
            return;
        }

        transform.rotation = Quaternion.Lerp(transform.rotation, _angle, Time.deltaTime * speed);
        
	}
}
