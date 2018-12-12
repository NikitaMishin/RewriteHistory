using System;
using UnityEngine;


public class OrdinaryPlayerController : MonoBehaviour
{
    /*
     * For moving in 2axis
     * add script to player
     * add managerController to player
     * add characterController to player
     */
    private ManagerController _managerController;

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
        _managerController = GetComponent<ManagerController>();
    }

    private void OnDisable()
    {
        _managerController.SetActualSpeed(_currentActualSpeed);
    }

    void Update()
    {
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

    /// <summary>
    /// Move forward according to all forces,speeds,inertia that had been applied
    /// </summary>
    private void PressRightMove()
    {
        if (!_managerController.direction)
        {
            // Rotate
            if (_currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
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

    private void PressLeftMove()
    {
        if (_managerController.direction)
        {
            if (_currentActualSpeed <= _managerController.OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
                _managerController.direction = !_managerController.direction; //TODO what the fuck
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

        dirVector += transform.forward * _currentActualSpeed;
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
}