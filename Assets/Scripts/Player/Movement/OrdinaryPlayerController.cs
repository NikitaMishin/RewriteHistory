using System;
using ReverseTime;
using UnityEngine;


public class OrdinaryPlayerController : MonoBehaviour, IRevertListener, IController
{
    /*
     * For moving in 2axis
     * add script to player
     * add managerController to player
     * add characterController to player
     */

    [SerializeField]
    protected float jumpTimeWithoutGround = 0.2f;

    public float walkingAnimationSpeed = 5;


    protected ManagerController _managerController;
    protected ManagerStates _managerStates;

    protected CharacterController _controller;
    protected Vector3 dirVector = Vector3.zero; // direction vector

    //FOR DASH
    protected float _currentDashTime = 0f; // in what period we press dash
    protected float _remainDash; // delta between maxDash and NormalSpeed


    // FOR CROUCH
    protected Transform _tMesh; // Player Transform
    protected float _characterHeight;
    protected Vector3 _initialLocalScale;

    // FOR JUMP
    protected float jumpPressTime = -1; // when Jump button was pressed

    protected bool prevGround = false;
    protected bool wasJumped = false;
    protected float prevTime = 0;

    protected bool charOnTheGround;
   
    protected TimeControllerPlayer _timeController;

    protected bool wasStopped = false;

    protected Quaternion lastRotation;

    void Awake() {
        lastRotation = Quaternion.Euler(new Vector3(0, 90, 0));
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
        _managerController.SetActualSpeed(_managerController._currentActualSpeed);
    }

    void Update()
    {
        charOnTheGround = IsOnTheGround();

     //   transform.rotation = lastRotation;
        
        if (prevGround && !charOnTheGround && Mathf.Abs(_managerController.jSpeed) < 1f && !wasJumped)
        {
            prevTime = Time.time;
        }

        if (charOnTheGround)
        {
            wasJumped = false;
        }

        prevGround = charOnTheGround;
    }

    public bool IsOnTheGround()
    {
        // maybe add custom detection
        if (_controller.isGrounded && _managerController.jSpeed < -2)
            _managerController.jSpeed = 0;
        return _controller.isGrounded;
    }

    /// <summary>
    /// Move forward according to all forces,speeds,inertia that had been applied
    /// </summary>
    protected void PressRightMove()
    {
        if (!_managerController.direction)
        {
            // Rotate
            if (_managerController._currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
                lastRotation = transform.rotation;
                _managerController.direction = !_managerController.direction;
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

    protected void PressLeftMove()
    {
        if (_managerController.direction)
        {
            if (_managerController._currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
                lastRotation = transform.rotation;
                _managerController.direction = !_managerController.direction;
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

        dirVector += transform.forward * _managerController._currentActualSpeed;
    }


    protected void ApplyInertia()
    {
        float inertiaForce = _managerController._currentActualSpeed / 2f;
        dirVector += transform.forward * inertiaForce;

        _managerController._currentActualSpeed =
            Math.Max(0f,
                _managerController._currentActualSpeed - _managerController._currentNormalSpeed *
                _managerController.InertiaStopPercentCoef);
    }


    protected void Jump()
    {
        _managerController.jSpeed += _managerController.JumpSpeed;
    }

    /// <summary>
    /// Update _currentDashTime,_currentNormalSpeed according to DashLimitSec when user press Dash button
    /// </summary>
    protected void DashPressed()
    {
        if (_currentDashTime < _managerController.DashLimitSec)
        {
            _currentDashTime += Time.deltaTime;
            _managerController._currentNormalSpeed = _managerController.DashMaxSpeed;
        }
        else
        {
            _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
        }
    }

    /// <summary>
    /// Update _currentDashTime according to currentNormalSpeed when user not hold Dash button
    /// </summary>
    protected void RestoreDashTime()
    {
        if (_managerController._currentActualSpeed <= _managerController._currentNormalSpeed)
        {
            //we cover and restore
            _currentDashTime = Mathf.Max(0f, _currentDashTime - Time.deltaTime); //player rest             
        }
    }

    protected void InitialSpeedSetup()
    {
        bool charOnTheGround = IsOnTheGround();
        if (charOnTheGround)
        {
            //if character on the ground acceleration=0
            _managerController.jSpeed = 0;
            _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
        }


        if (!charOnTheGround)
        {
            _managerController._currentNormalSpeed = _managerController.SpeedInAir;
        }

        if (_managerController.isCrouch)
        {
            _managerController._currentNormalSpeed = _managerController.CrouchSpeed;
        }
    }

    /// <summary>
    /// Update controller properties and _current Normal speed based on isCrouchStatus when button pressed
    /// </summary>
    protected void CrouchPressed()
    {
       // AnimateWalking();
        if (!_managerController.isCrouch && IsOnTheGround())
        {
            _tMesh.localScale = new Vector3(_initialLocalScale.x, _managerController.LocalScaleY, _initialLocalScale.z);
            _controller.height = _managerController.ControllerHeight;
            _managerController._currentNormalSpeed = _managerController.CrouchSpeed;
            //    _managerController._currentActualSpeed = _managerController.CrouchSpeed > _managerController._currentActualSpeed
            //        ? _managerController._currentActualSpeed : _managerController.CrouchSpeed;
            _managerController._currentActualSpeed = 0;
            _managerController.isCrouch = true;
            _controller.center = new Vector3(0, _controller.center.y - (_characterHeight - _managerController.ControllerHeight) / 2, 0);
        }
    }

    /// <summary>
    /// Update controller properties and _currentNormal speed based on isCrouchStatus when button released and
    /// we actually can stand up
    /// </summary>
    protected void CrouchStop()
    {
      //  return;
        Ray ray = new Ray();
        RaycastHit hit;
        ray.origin = _controller.bounds.max;
        ray.direction = Vector3.up;

        if (Physics.Raycast(ray, out hit, 0.3f)) return;

        // we can stand up
        _tMesh.localScale = _initialLocalScale;
        _controller.height = _characterHeight;
        _managerController.isCrouch = false;
        _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
        _controller.center = new Vector3(0, _controller.center.y + (_characterHeight - _managerController.ControllerHeight) / 2, 0);
    }

    public void SetActualSpeed(float speed)
    {
        _managerController._currentActualSpeed = speed;
    }

    public float GetActualSpeed()
    {
        return _managerController._currentActualSpeed;
    }

    public ManagerController GetManagerController()
    {
        return _managerController;
    }

    public void SetManagerController(ManagerController managerController)
    {
        _managerController = managerController;
    }

    public void RecordTimePoint()
    {
        _managerController.TimePoints.AddLast(new OrdinaryPlayerControllerTimePoint
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
                jSpeed = _managerController.jSpeed,
                state = _managerStates.GetCurrentState()
            }
        );
        
    }

    public void StartRewind()
    {
        if (_managerController.TimePoints.Count <= 0)
        {
            return;
        }

     //   _controller.enabled = false;

        OrdinaryPlayerControllerTimePoint timePoint =
            (OrdinaryPlayerControllerTimePoint) _managerController.TimePoints.Last.Value;

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
        _managerController.jSpeed = timePoint.jSpeed;

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

    public float GetFallSpeed()
    {
        return _managerController.jSpeed;
    }

    void IController.Jump()
    {
        _managerController.isReadyToJump = true;
        jumpPressTime = Time.time;

        if ((charOnTheGround || Time.time - prevTime < 0.5f || _managerController.IsOnTheIncline) && _managerController.isReadyToJump && !_managerController.isCrouch)
        {
            wasJumped = true;
            prevTime = 0;
            Jump();
            _managerController.isReadyToJump = false;
        }

    }

    private void JumpInvoke()
    {
       
    }

    public void CrouchStart()
    {
        CrouchPressed();
    }

    void IController.CrouchStop()
    {
        if (_managerController.isCrouch)
        {
            CrouchStop();
        }
    }

    public void Dash()
    {
        if (charOnTheGround && !_managerController.isCrouch && _managerController.canDash)
        {
            DashPressed();
            _managerController._isDashPressed = true;
        }
        else
        {
            RestoreDashTime();
            _managerController._isDashPressed = false;
        }
    }

    public void RestartDash()
    {
        RestoreDashTime();
        _managerController._isDashPressed = false;
    }

    public void RightMove() {
        wasStopped = false;
        PressRightMove();
    }

    public void LeftMove()
    {
        wasStopped = false;
        PressLeftMove();
    }

    public void StopJump()
    {
        if (Time.time - jumpPressTime > 0.3)
        {
            _managerController.isReadyToJump = false;
        }
    }

    public void StopActualSpeed() {
        wasStopped = true;
        _managerController._currentActualSpeed = 0;//_managerController._currentActualSpeed > 0 ? _managerController._currentActualSpeed - _managerController.inertia : (_managerController._currentActualSpeed < 0 ? 0 : 0) ;
    }

    public void Move()
    {
        _managerController.jSpeed += _managerController.Gravity * Time.deltaTime * _managerController.FallSpeed;
        dirVector = (dirVector + Vector3.up * _managerController.jSpeed + _managerController.forceVector) * Time.deltaTime;
        _controller.Move(dirVector);
    }

    public void RestartDir()
    {
        dirVector = Vector3.zero;

    /*    if (wasStopped)
        {
            dirVector.x = _managerController._currentActualSpeed * transform.forward.x;
            dirVector.y = _managerController._currentActualSpeed * transform.forward.y;
        }
        */
        InitialSpeedSetup();
    }
}