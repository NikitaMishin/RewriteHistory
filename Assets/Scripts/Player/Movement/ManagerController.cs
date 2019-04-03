﻿using System;
using System.Collections;
using System.Collections.Generic;
using ReverseTime;
using UnityEngine;
public class ManagerController : MonoBehaviour, IRevertListener
{
    /*
     * add script to player
     * set constant as u wish
     * 
     */
    
    //

    // DIRECTION
    public bool direction = true; // for moving along axis (need for rotating) true for positive false for negative

    public bool _isDashPressed = false;
    public bool isCrouch = false;
    public float _currentNormalSpeed; // depends on isGrounded and Crouch status 

    // DASH
    public float DashLimitSec = 5f; // interval when user can dash;ус
    public float DashMaxSpeed = 12f; // maximum speed on dash 
    public float DashAccelerationPercent = 1f; //  how fast we reach limit when dash pressed
    public bool canDash = true;

    //GRAVITY
    public float Gravity = -9.81f;
    public float FallSpeed = 1.5f; // how fast we fall 

    //SPEED On GROUND
    public float SpeedOnGround = 6f; // velocity when on ground

    //CROUCH
    public float CrouchSpeed = 3f; // speed when crouch
    public float LocalScaleY; // Y scale of tMesh aka local height when he crouch
    public float ControllerHeight; // Y scale of the character controller when he crouch


    // JUMP and air
    public float JumpSpeed = 8f; //height of jump
    public float SpeedInAir = 4f; // velocity when in air
    public bool isReadyToJump = false;


    //Inertia and 
    public float OnWhichSpeedCanRotate = 0f; //we will apply inertia until we hit this floor and only then can rotate 
    public float InertiaStopPercentCoef = 0.33f; // how fast we stop  for inertia

    public float
        SpeedAccelerationPercent = 0.23f; // percent of current normal speed that we apply to actualSpeed to reach limit


    public float ThresholdPercent = 0.15f; //if abs(_actualSpeed - normalSpeed)/Max(_actualSpeed,normalSpeed)<threshold
    // then we assign normal speed to actual
    // must be between 0 and 1

    public float _currentActualSpeed = 0; // actual speed

    public bool onlySlide = false;

    public Animator animator;

    public TimeControllerPlayer _timeController;
    private OrdinaryPlayerController _ordinaryPlayerController;
    private BezierCurvePlayerController _bezierCurvePlayerController;
    private StairController _stairController;
    private MoveObjectController _moveObjectController;

    private bool _isOnTheIncline = false;

    private Signals currentSignal = Signals.ActivatePlayerController;

    public Vector3 forceVector = Vector3.zero; // player forces

    public float jSpeed = 0;

    public LinkedList<ITimePoint> TimePoints; //storage where we save user move
    private bool _wasInit = false;

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        if (_wasInit)
            return;

        _wasInit = true;

        _timeController = gameObject.GetComponent<TimeControllerPlayer>();
        _ordinaryPlayerController = GetComponent<OrdinaryPlayerController>();
        _bezierCurvePlayerController = GetComponent<BezierCurvePlayerController>();
        _stairController = GetComponent<StairController>();
        TimePoints = new LinkedList<ITimePoint>();
        _moveObjectController = GetComponent<MoveObjectController>();

        animator = gameObject.GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        if (ShouldRewind())
        {
            //start reverse
            StartRewind();
        }
        else if (ShouldClearWhenOtherRewind())
        {
            DeleteOldRecord();
            DeleteOldRecord();
            RecordTimePoint();
        }
        else
        {
            //record point
            RecordTimePoint();
        }

        if (_timeController.ShouldRemoveOldRecord())
        {
            DeleteOldRecord();
        }
        
        //  IsOnTheGround();
    }

    /// <summary>
    /// SIGNAL ACTIVATE scripts for specific movement and action
    /// 
    /// </summary>
    /// <param name="signal"></param>
    public void SendSignal(Signals signal)
    {
        DisableControllers();

        currentSignal = signal;

        switch (signal)
        {
            case Signals.ActivatePlayerController:
                _ordinaryPlayerController.SetActualSpeed(_currentActualSpeed);
                _ordinaryPlayerController.enabled = true;
                break;
            case Signals.ActivateBezierController:
                _bezierCurvePlayerController.SetActualSpeed(_currentActualSpeed);
                _bezierCurvePlayerController.enabled = true;
                break;
            case Signals.ActivateStairsController:
                _stairController.enabled = true;
                break;
            case Signals.ActivateMoveObjectController:
                _moveObjectController.SetActualSpeed(_currentActualSpeed);
                _moveObjectController.enabled = true;
                break;
            default: throw new NotImplementedException();
        }
    }

    private void DisableControllers()
    {
        _ordinaryPlayerController.enabled = false;
        _bezierCurvePlayerController.enabled = false;
        _stairController.enabled = false;
        _moveObjectController.enabled = false;
    }

    public void SetActualSpeed(float speed)
    {
        _currentActualSpeed = speed;
    }

    public void RecordTimePoint()
    {
        //determine from what script we record
        if (_ordinaryPlayerController.enabled)
        {
            _ordinaryPlayerController.RecordTimePoint();
        }
        else if (_bezierCurvePlayerController.enabled)
        {
            _bezierCurvePlayerController.RecordTimePoint();
        }
        else if (_stairController.enabled)
        {
            _stairController.RecordTimePoint();
        }
        else if (_moveObjectController.enabled)
        {
            _moveObjectController.RecordTimePoint();
        }
    }

    public bool IsOnTheGround()
    {
        bool isOnTheGround = false;

        //determine from what script we record
        if (_ordinaryPlayerController.enabled)
        {
            isOnTheGround =_ordinaryPlayerController.IsOnTheGround();
        }
        else if (_bezierCurvePlayerController.enabled)
        {
            isOnTheGround = _bezierCurvePlayerController.IsOnTheGround();
        }
        else if (_stairController.enabled)
        {
            isOnTheGround = _stairController.IsOnTheGround();
        }
        else if (_moveObjectController.enabled)
        {
            isOnTheGround = _moveObjectController.IsOnTheGround();
        }

    //    if (isOnTheGround)
     //       jSpeed = 0;

        return isOnTheGround;
    }

    public void StartRewind()
    {
        if (TimePoints.Count == 0)
        {
            return;
        }


        var timePoint = TimePoints.Last.Value;
        if (timePoint is OrdinaryPlayerControllerTimePoint)
        {
            //disable bezier
            //_bezierCurvePlayerController.enabled = false;
            //_ordinaryPlayerController.enabled = true;
            _ordinaryPlayerController.StartRewind();
        }
        else if (timePoint is BezierCurvePlayerControllerTimePoint)
        {
            //disable bezier
            //_bezierCurvePlayerController.enabled = true;
            //_ordinaryPlayerController.enabled = false;
            _bezierCurvePlayerController.StartRewind();
        }
        else if (timePoint is StairPlayerControllerTimePoint)
        {
            _stairController.StartRewind();
        }
    }

    public Signals CurrentSignal
    {
        get { return currentSignal; }
    }

    public bool IsOnTheIncline
    {
        get
        {
            return _isOnTheIncline;
        }

        set
        {
            _isOnTheIncline = value;
        }
    }

    public void DeleteOldRecord()
    {
        if (TimePoints.Count > 0)
        {
            TimePoints.RemoveFirst();
        }
    }

    public void DeleteAllRecord()
    {
        throw new NotImplementedException();
    }

    public bool ShouldRewind()
    {
        return _timeController.IsReversing && _timeController.IsUserShouldReverse;
    }

    private bool ShouldClearWhenOtherRewind()
    {
        return _timeController.IsReversing && !_timeController.IsUserShouldReverse;
    }
    
}