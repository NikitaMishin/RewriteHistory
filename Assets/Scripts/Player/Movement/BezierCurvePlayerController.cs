using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using ReverseTime;
using UnityEngine;

public class BezierCurvePlayerController : OrdinaryPlayerController, IRevertListener, IController
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

    public int CurrentWayPointId = 0;
    public float RotationSpeed = 3f;
    public float ReachDistance = 0.5f;

    public List<Vector3> CurvePoints;
 
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _tMesh = GetComponent<Transform>();
        _managerController = GetComponent<ManagerController>();
        _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
        _managerController._currentActualSpeed = 0f;
        _remainDash = Mathf.Max(_managerController.DashMaxSpeed - _managerController._currentNormalSpeed, 0);
        _characterHeight = _controller.height;
        _initialLocalScale = _tMesh.localScale;
        _timeController = FindObjectOfType<TimeControllerPlayer>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    private void OnDisable()
    {
        // set actual speed in manager
        _managerController.SetActualSpeed(_managerController._currentActualSpeed);
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

        if (_managerStates.GetCurrentState() == State.Dead)
            return;

        charOnTheGround = IsOnTheGround();
    }

    private void PressRightMove()
    {
        if (!_managerController.direction)
        {
            // Rotate
            if (_managerController._currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                //  transform.rotation *= Quaternion.Euler(0, 180f, 0);
                _managerController.direction = !_managerController.direction;
                directionCurve = !directionCurve;
                UpdateIndexPoint();
                if (!_managerController.onlySlide)
                    MoveForward();
            }
            else
            {
             //   ApplyInertia();
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
            if (_managerController._currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                // transform.rotation *= Quaternion.Euler(0, 180f, 0);
                _managerController.direction = !_managerController.direction;
                directionCurve = !directionCurve;
                UpdateIndexPoint();

                if (!_managerController.onlySlide)
                    MoveForward();
            }
            else
            {
            //    ApplyInertia();
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
        if (_managerController._currentActualSpeed < _managerController._currentNormalSpeed)
        {
            if (_managerController._isDashPressed)
            {
                _managerController._currentActualSpeed +=
                    _managerController.DashAccelerationPercent * _remainDash * Time.deltaTime;
            }
            else
            {
                _managerController._currentActualSpeed +=
                    _managerController.SpeedAccelerationPercent * _managerController._currentNormalSpeed *
                    Time.deltaTime;
            }
        }
        else if (_managerController._currentActualSpeed > _managerController._currentNormalSpeed)
        {
            _managerController._currentActualSpeed -= _managerController.SpeedAccelerationPercent *
                                                      _managerController._currentNormalSpeed * Time.deltaTime;
        }

        float percent = Math.Abs(_managerController._currentActualSpeed - _managerController._currentNormalSpeed) /
                        Math.Max(_managerController._currentNormalSpeed, _managerController._currentActualSpeed);
        if (percent < _managerController.ThresholdPercent)
        {
            _managerController._currentActualSpeed = _managerController._currentNormalSpeed;
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
            dirVector += lookPos.normalized * _managerController._currentActualSpeed;
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
            dirVector += lookPos.normalized * _managerController._currentActualSpeed;
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

        dirVector += lookPos.normalized * _managerController._currentActualSpeed;
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
                isCrouch = _managerController.isCrouch,
                _currentActualSpeed = _managerController._currentActualSpeed,
                _currentDashTime = _currentDashTime,
                jumpPressTime = jumpPressTime,
                isReadyToJump = _managerController.isReadyToJump,
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
        _managerController.isCrouch = timePoint.isCrouch;
        _managerController._currentActualSpeed = timePoint._currentActualSpeed;
        jumpPressTime = timePoint.jumpPressTime;
        _managerController.isReadyToJump = timePoint.isReadyToJump;
        _currentDashTime = timePoint._currentDashTime;
        _managerController.direction = timePoint.Direction;
        _tMesh.localScale = timePoint.localScale;


        directionCurve = timePoint.directionCurve;
        CurrentWayPointId = timePoint.CurrentWayPointId;

        _managerStates.ChangeState(State.Default);
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
        throw new NotImplementedException();
    }

    public void RightMove()
    {
        wasStopped = false;

        PressRightMove();
    }

    public void LeftMove()
    {
        wasStopped = false;

        PressLeftMove();
    }
}