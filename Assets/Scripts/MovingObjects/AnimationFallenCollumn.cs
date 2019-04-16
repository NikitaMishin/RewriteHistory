using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationFallenCollumn : AnimationRewind {

    [SerializeField] protected float timeAfterFallNext = 0.2f;

    private ManagerStates _managerStates;

    private void Start()
    {
        _animation = gameObject.transform.parent.gameObject.GetComponent<Animation>();
        _managerStates = FindObjectOfType<ManagerStates>();
    }

    private void Update()
    {
        //if (_managerStates.GetCurrentState() == State.Dead)
        //    _animation[nameAnimation].time = 0;


        if (Time.time - _lastTime > timeAfterFallNext && _timeToRun)
        {
            _animation[nameAnimation].speed = 1;
            _timeToRun = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && (!_wasStepped || _animation[nameAnimation].speed != 0))
        {
            _wasStepped = true;
            _animation.Play();
            _animation[nameAnimation].speed = 1;
        }
    }
}
