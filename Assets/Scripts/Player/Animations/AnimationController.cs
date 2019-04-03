﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    private ManagerController _managerController;
    private ManagerStates _managerStates;
    private TimeControllerObject _timeControllerObject;

    // Use this for initialization
    void Start () {
        _managerController = gameObject.GetComponent<ManagerController>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
        _timeControllerObject = FindObjectOfType<TimeControllerObject>();
    }
	
	// Update is called once per frame
	void Update () {
        SetFallSpeed();
        AnimateMovement();
        AnimateFalling();
        AnimateJumping();
        AnimateCrouch();
        AnimateDead();
        AnimateReversing();
	}

    private void AnimateReversing()
    {
        if (_timeControllerObject.IsReversing)
            _managerController.animator.speed = 0;
        else
            _managerController.animator.speed = 1;
    }

    private void AnimateDead()
    {
        if (_managerStates.GetCurrentState() == State.Dead)
            _managerController.animator.SetBool("Jump", false);
    }

    private void AnimateFalling()
    {
        if (_managerController.jSpeed < -3)
        {
            if (!_managerController.animator.GetBool("IsFalling"))
                _managerController.animator.SetBool("IsFalling", true);

        }
        else
        {
            _managerController.animator.SetBool("IsFalling", false);
        }
    }

    private void AnimateJumping()
    {

        if (_managerController.jSpeed > 0)
        {
            _managerController.animator.SetBool("Jump", true);
        }
        else if (_managerController.IsOnTheGround())
        {
            _managerController.animator.SetBool("Jump", false);
        }

    }

    private void AnimateCrouch()
    {
        if (_managerController.isCrouch || _managerController.onlySlide && _managerController.IsOnTheIncline)
            _managerController.animator.SetBool("IsCrouching", true);
        else
            _managerController.animator.SetBool("IsCrouching", false);
    }

    private void SetFallSpeed()
    {
        if (!_managerController.onlySlide)
            _managerController.animator.SetFloat("FallSpeed", _managerController.jSpeed);
        else
            _managerController.animator.SetFloat("FallSpeed", 0);
    }

    private void AnimateMovement()
    {
        if (!_managerController.onlySlide)
            _managerController.animator.SetFloat("MovementSpeed", _managerController._currentActualSpeed);
        else
            _managerController.animator.SetFloat("MovementSpeed", 0);
    }
}
