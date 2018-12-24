using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ReverseTime;
using UnityEngine;

public class BezierCurvePlayerController : MonoBehaviour, IRevertListener
{
    /*
     *HOW TO
     * add script manager Controller to player
     * add this script to player
     * add characterController script to player
     * directionCurve  is direction of curve
     * RotationSpeed speed of rotation when move along curve
     * ReachDistance on which delta we can set  new target point on curve
     */


    public bool directionCurve = true; // for moving from start to end or end to start
    public BezierCurve Curve; //curve along we move// can setup

    public int CurrentWayPointId = 0;
    public float RotationSpeed = 3f;
    public float ReachDistance = 0.5f;

    private ManagerController _managerController;

    // could be used from other scripts like rotate on triggers

    //PRIVATE VARS

    public List<Vector3> CurvePoints;
    private CharacterController _controller;
    private Vector3 dirVector = Vector3.zero; // direction vector
    private float _currentNormalSpeed; // depends on isGrounded and Crouch status 
    private float _currentActualSpeed; // actual speed 


    //FOR DASH
    private float _currentDashTime = 0f; // in what period we press dash
    private float _remainDash; // delta between maxDash and NormalSpeed
    private bool _isDashPressed = false;


    // FOR CROUCH
    private bool isCrouch = false;
    private Transform _tMesh; // Player Transform
    private float _characterHeight;
    private Vector3 _initialLocalScale;


    // FOR JUMP and gravity
    private bool isReadyToJump = true;
    private float jumpPressTime = -1; // when Jump button was pressed
    private float _jSpeed = 0; // initial y axis speed;


    private TimeController _timeController;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _tMesh = GetComponent<Transform>();
        _managerController = GetComponent<ManagerController>();
        _currentNormalSpeed = _managerController.SpeedOnGround;
        _currentActualSpeed = 0f;
        _remainDash = Mathf.Max(_managerController.DashMaxSpeed - _currentNormalSpeed, 0);
        _characterHeight = _controller.height;
        _initialLocalScale = _tMesh.localScale;
        _timeController = FindObjectOfType<TimeController>();
       // if (Curve.close)
       // {
       //     throw new Exception("USER COULDONT MOVE ON CLOSED PATH.PLEASE UNMARK CLOSE PROPERTY");
       // }
    }


    private void OnDisable()
    {
        // set actual speed in manager
        _managerController.SetActualSpeed(_currentActualSpeed);
    }

    private void UpdateIndexPoint()
    {
        //directionCurve=true when index from left to right

        if (directionCurve && CurrentWayPointId == CurvePoints.Count - 1)
        {
            // reach last point
            //this.enabled = false;
            //directionCurve = !directionCurve;
            CurrentWayPointId = CurvePoints.Count;
        }
        else if (!directionCurve && CurrentWayPointId == 0)
        {
            CurrentWayPointId = -1;
            //reach first point
            //this.enabled = false;
            //directionCurve = !directionCurve;
        }
        else if (directionCurve)
        {
            CurrentWayPointId++;
        }
        else if (!directionCurve)
        {
            CurrentWayPointId--;
        }
    }


    void Update()
    {
        if (_managerController.ShouldRewind()) return;
        bool charOnTheGround = IsOnTheGround();

        dirVector = Vector3.zero;

        InitialSpeedSetup();

        if (Input.GetKey(KeyCode.S))
        {
            CrouchPressed();
        }
        else if (isCrouch)
        {
            CrouchStop();
        }


        if (charOnTheGround && Input.GetKey(KeyCode.LeftShift) && !isCrouch)
        {
            DashPressed();
            _isDashPressed = true;
        }
        else
        {
            RestoreDashTime();
            _isDashPressed = false;
        }


        if (Input.GetKey(KeyCode.D))
        {
            PressRightMove();
        }

        if (Input.GetKey(KeyCode.A))
        {
            PressLeftMove();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            isReadyToJump = true;
            jumpPressTime = Time.time;
        }
        else if (Input.GetKeyUp(KeyCode.W))
        {
            isReadyToJump = false;
        }
        else if (Time.time - jumpPressTime > 0.3)
        {
            isReadyToJump = false;
        }

        if (charOnTheGround && isReadyToJump)
        {
            Jump();
            isReadyToJump = false;
        }


        _jSpeed += _managerController.Gravity * Time.deltaTime * _managerController.FallSpeed;

        dirVector = (dirVector + Vector3.up * _jSpeed) * Time.deltaTime;

        _controller.Move(dirVector);
        //UpdateCameraPosition();
    }

    public bool IsOnTheGround()
    {
        // maybe add custom detection
        return _controller.isGrounded;
    }

    private void PressRightMove()
    {
        if (!_managerController.direction)
        {
            // Rotate
            if (_currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                //  transform.rotation *= Quaternion.Euler(0, 180f, 0);
                _managerController.direction = !_managerController.direction;
                directionCurve = !directionCurve;
                UpdateIndexPoint();
                MoveForward();
            }
            else
            {
                ApplyInertia();
            }
        }
        else
        {
            MoveForward();
        }
    }

    private void PressLeftMove()
    {
        if (_managerController.direction)
        {
            if (_currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                // transform.rotation *= Quaternion.Euler(0, 180f, 0);
                _managerController.direction = !_managerController.direction;
                directionCurve = !directionCurve;
                UpdateIndexPoint();
                MoveForward();
            }
            else
            {
                ApplyInertia();
            }
        }
        else
        {
            MoveForward();
        }
    }

    private void MoveForward()
    {
        // continue move along direction vector
        if (_currentActualSpeed < _currentNormalSpeed)
        {
            if (_isDashPressed)
            {
                _currentActualSpeed += _managerController.DashAccelerationPercent * _remainDash * Time.deltaTime;
            }
            else
            {
                _currentActualSpeed +=
                    _managerController.SpeedAccelerationPercent * _currentNormalSpeed * Time.deltaTime;
            }
        }
        else if (_currentActualSpeed > _currentNormalSpeed)
        {
            _currentActualSpeed -= _managerController.SpeedAccelerationPercent * _currentNormalSpeed * Time.deltaTime;
        }

        float percent = Math.Abs(_currentActualSpeed - _currentNormalSpeed) /
                        Math.Max(_currentNormalSpeed, _currentActualSpeed);
        if (percent < _managerController.ThresholdPercent)
        {
            _currentActualSpeed = _currentNormalSpeed;
        }


        ApplyMoveBezier();
    }

    private void ApplyMoveBezier()
    {
        Vector3 lookPos;
        Quaternion rotation;

        if (CurrentWayPointId == -1 && directionCurve)
        {
            // move to curve  start
            CurrentWayPointId++;
        }
        else if (CurrentWayPointId == -1 && !directionCurve)
        {
            // move in opposite side with inverse rotation
            lookPos = CurvePoints[0] - CurvePoints[1];
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos); // rotation in zx plane
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            dirVector += lookPos.normalized * _currentActualSpeed;
            return;
        }
        else if (CurrentWayPointId == CurvePoints.Count - 1 && !directionCurve)
        {
            // move to curve end
            CurrentWayPointId--;
        }
        else if (CurrentWayPointId == CurvePoints.Count && directionCurve)
        {
            // move in opposite side with inverse rotation
            lookPos = CurvePoints[CurvePoints.Count - 1] - CurvePoints[CurvePoints.Count - 2];
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos); // rotation in zx plane
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
            dirVector += lookPos.normalized * _currentActualSpeed;
            return;
        }


        //проверяем без y
        Vector3 pos = transform.position;
        pos.y = 0;
        Vector3 curWayPoint = CurvePoints[CurrentWayPointId];
        curWayPoint.y = 0;

        float distance = Vector3.Distance(curWayPoint, pos); //without y axis

        //ROTATION
        lookPos = CurvePoints[CurrentWayPointId] - transform.position;
        lookPos.y = 0;
        rotation = Quaternion.LookRotation(lookPos); // rotation in zx plane
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);
        //ROTATION

        if (distance <= ReachDistance)
        {
            //update next target point on curve
            UpdateIndexPoint();
        }

        dirVector += lookPos.normalized * _currentActualSpeed;
    }


    private void ApplyInertia()
    {
        float inertiaForce = _currentActualSpeed / 2f;
        dirVector += transform.forward * inertiaForce;

        _currentActualSpeed =
            Math.Max(0f, _currentActualSpeed - _currentNormalSpeed * _managerController.InertiaStopPercentCoef);
    }


    private void Jump()
    {
        _jSpeed += _managerController.JumpSpeed;
    }

    /// <summary>
    /// Update _currentDashTime,_currentNormalSpeed according to DashLimitSec when user press Dash button
    /// </summary>
    private void DashPressed()
    {
        if (_currentDashTime < _managerController.DashLimitSec)
        {
            _currentDashTime += Time.deltaTime;
            _currentNormalSpeed = _managerController.DashMaxSpeed;
        }
        else
        {
            _currentNormalSpeed = _managerController.SpeedOnGround;
        }
    }

    /// <summary>
    /// Update _currentDashTime according to currentNormalSpeed when user not hold Dash button
    /// </summary>
    private void RestoreDashTime()
    {
        if (_currentActualSpeed <= _currentNormalSpeed)
        {
            //we cover and restore
            _currentDashTime = Mathf.Max(0f, _currentDashTime - Time.deltaTime); //player rest             
        }
    }

    private void InitialSpeedSetup()
    {
        bool charOnTheGround = IsOnTheGround();
        if (charOnTheGround)
        {
            //if character on the ground acceleration=0
            _jSpeed = 0;
            _currentNormalSpeed = _managerController.SpeedOnGround;
        }


        if (!charOnTheGround)
        {
            _currentNormalSpeed = _managerController.SpeedInAir;
        }

        if (isCrouch)
        {
            _currentNormalSpeed = _managerController.CrouchSpeed;
        }
    }

    /// <summary>
    /// Update controller properties and _current Normal speed based on isCrouchStatus when button pressed
    /// </summary>
    private void CrouchPressed()
    {
        if (!isCrouch && IsOnTheGround())
        {
            _tMesh.localScale = new Vector3(_initialLocalScale.x, _managerController.LocalScaleY, _initialLocalScale.z);
            _controller.height = _managerController.ControllerHeight;
            _currentNormalSpeed = _managerController.CrouchSpeed;
            isCrouch = true;
        }
    }

    /// <summary>
    /// Update controller properties and _currentNormal speed based on isCrouchStatus when button released and
    /// we actually can stand up
    /// </summary>
    private void CrouchStop()
    {
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = transform.position;
        ray.direction = Vector3.up;

        if (Physics.Raycast(ray, out hit, 1)) return;

        // we can stand up
        _tMesh.localScale = _initialLocalScale;
        _controller.height = _characterHeight;
        isCrouch = false;
        _currentNormalSpeed = _managerController.SpeedOnGround;
    }

    public void SetActualSpeed(float speed)
    {
        _currentActualSpeed = speed;
    }

    public float GetActualSpeed()
    {
        return _currentActualSpeed;
    }

    public void RecordTimePoint()
    {
        _managerController.TimePoints.AddLast(new BezierCurvePlayerControllerTimePoint
            {
                position = transform.position,
                rotation = transform.rotation,
                _characterHeight = _characterHeight,
                _remainDash = _remainDash,
                _tMesh = _tMesh,
                isCrouch = isCrouch,
                _currentActualSpeed = _currentActualSpeed,
                _currentDashTime = _currentDashTime,
                jumpPressTime = jumpPressTime,
                isReadyToJump = isReadyToJump,
                Direction = _managerController.direction,
                localScale = _tMesh.localScale,
                directionCurve = directionCurve,
                CurrentWayPointId = CurrentWayPointId
            }
        );
    }

    public void StartRewind()
    {
        if (_managerController.TimePoints.Count <= 0)
        {
            return;
        }

        var timePoint = (BezierCurvePlayerControllerTimePoint) _managerController.TimePoints.Last.Value;

        //apply rewind
        transform.position = timePoint.position;
        transform.rotation = timePoint.rotation;
        _characterHeight = timePoint._characterHeight;
        _remainDash = timePoint._remainDash;
        _tMesh = timePoint._tMesh;
        isCrouch = timePoint.isCrouch;
        _currentActualSpeed = timePoint._currentActualSpeed;
        jumpPressTime = timePoint.jumpPressTime;
        isReadyToJump = timePoint.isReadyToJump;
        _currentDashTime = timePoint._currentDashTime;
        _managerController.direction = timePoint.Direction;
        _tMesh.localScale = timePoint.localScale;


        directionCurve = timePoint.directionCurve;
        CurrentWayPointId = timePoint.CurrentWayPointId;


        // delete Point

        _managerController.TimePoints.RemoveLast();
    }

    public void DeleteOldRecord()
    {
        //manager handle this
        throw new NotImplementedException();
    }

    public void DeleteAllRecord()
    {
        //manager handle this
        throw new NotImplementedException();
    }

    public bool ShouldRewind()
    {
        //manager handle this
        throw  new NotImplementedException();
    }
}