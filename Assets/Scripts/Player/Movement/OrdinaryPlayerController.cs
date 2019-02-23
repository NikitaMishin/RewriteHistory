using System;
using ReverseTime;
using UnityEngine;


public class OrdinaryPlayerController : MonoBehaviour, IRevertListener
{
    /*
     * For moving in 2axis
     * add script to player
     * add managerController to player
     * add characterController to player
     */

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
    protected bool isReadyToJump = true;
    protected float jumpPressTime = -1; // when Jump button was pressed
    protected float _jSpeed = 0; // initial y axis speed;

    protected TimeController _timeController;


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
        _timeController = FindObjectOfType<TimeController>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    private void OnDisable()
    {
        _managerController.SetActualSpeed(_managerController._currentActualSpeed);
    }

    private void AnimateWalking()
    {
        if (_managerController._currentActualSpeed == 0)
        {
            _managerController.animator.SetBool("IsRunning", false);
            _managerController.animator.SetBool("IsWalking", false);
        } else if (_managerController._currentActualSpeed < walkingAnimationSpeed)
        {
            _managerController.animator.SetBool("IsRunning", false);
            _managerController.animator.SetBool("IsWalking", true);
        }
        else
        {
            _managerController.animator.SetBool("IsWalking", false);
            _managerController.animator.SetBool("IsRunning", true);
        }
    }

    void DeadAnimation()
    {
        _managerController.animator.SetBool("IsWalking", false);
        _managerController.animator.SetBool("IsRunning", false);
        _managerController.animator.SetBool("Jump", false);
        _managerController.animator.SetBool("IsFalling", false);
    }

    void Update()
    {
        if (_managerController.ShouldRewind())
            return;

        _controller.enabled = true;

        if (_managerStates.GetCurrentState() == State.Dead)
        {
            DeadAnimation();
            return;
        }



        bool charOnTheGround = IsOnTheGround();

        if (charOnTheGround)
        {
            _managerController.animator.SetBool("IsFalling", false);
            _managerController.animator.SetBool("Jump", false);
        }
        else
        {
            _managerController.animator.SetBool("IsFalling", true);
        }
        
        dirVector = Vector3.zero;

        InitialSpeedSetup();

        if (Input.GetKey(KeyCode.S))
        {
            CrouchPressed();
        }
        else if (_managerController.isCrouch)
        {
            CrouchStop();
        }


        if (charOnTheGround && Input.GetKey(KeyCode.LeftShift) && !_managerController.isCrouch)
        {
            DashPressed();
            _managerController._isDashPressed = true;
        }
        else
        {
            RestoreDashTime();
            _managerController._isDashPressed = false;
        }


        if (Input.GetKey(KeyCode.D))
        {
            PressRightMove();
        }

        if (Input.GetKey(KeyCode.A))
        {
            PressLeftMove();
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            _managerController._currentActualSpeed = 0;

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

        if ((charOnTheGround || _managerController.IsOnTheIncline) && isReadyToJump)
        {
            Jump();
            _managerController.animator.SetBool("Jump", true);
            isReadyToJump = false;
        }


        _jSpeed += _managerController.Gravity * Time.deltaTime * _managerController.FallSpeed;
       
      
        // Animator
        if (IsOnTheGround())
            AnimateWalking();

        dirVector = (dirVector + Vector3.up * _jSpeed + _managerController.forceVector) * Time.deltaTime;

        _controller.Move(dirVector);
    }

    public bool IsOnTheGround()
    {
        // maybe add custom detection
        if (_controller.isGrounded && _jSpeed < -2)
            _jSpeed = 0;
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
                _managerController.direction = !_managerController.direction;
                MoveForward();
             //   _managerController.animator.SetBool("TurneRight", true);
            }
            else
            {
            //    _managerController.animator.SetBool("TurneRight", false);
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
                _managerController.direction = !_managerController.direction; //TODO what the fuck
                MoveForward();
              //  _managerController.animator.SetBool("TurneLeft", true);
            }
            else
            {
                _managerController.animator.SetBool("TurneLeft", false);
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


    private void ApplyInertia()
    {
        float inertiaForce = _managerController._currentActualSpeed / 2f;
        dirVector += transform.forward * inertiaForce;

        _managerController._currentActualSpeed =
            Math.Max(0f,
                _managerController._currentActualSpeed - _managerController._currentNormalSpeed *
                _managerController.InertiaStopPercentCoef);
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
            _jSpeed = 0;
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
    private void CrouchPressed()
    {
        if (!_managerController.isCrouch && IsOnTheGround())
        {
            _tMesh.localScale = new Vector3(_initialLocalScale.x, _managerController.LocalScaleY, _initialLocalScale.z);
            _controller.height = _managerController.ControllerHeight;
            _managerController._currentNormalSpeed = _managerController.CrouchSpeed;
            _managerController.isCrouch = true;
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
        _managerController.isCrouch = false;
        _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
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
                isReadyToJump = isReadyToJump,
                Direction = _managerController.direction,
                localScale = _tMesh.localScale,
                jSpeed = _jSpeed,
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

        _controller.enabled = false;

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
        isReadyToJump = timePoint.isReadyToJump;
        _currentDashTime = timePoint._currentDashTime;
        _managerController.direction = timePoint.Direction;
        _tMesh.localScale = timePoint.localScale;
        _jSpeed = timePoint.jSpeed;

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
        return _jSpeed;
    }
}