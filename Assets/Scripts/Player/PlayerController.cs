using System;
using UnityEngine;



public class PlayerController : MonoBehaviour
{
    /*
     *
     *
     *
     *
     * 
     */

    // DIRECTION
    public bool direction = true; // for moving along axis (need for rotating) true for positive false for negative


    // DASH
    public float DashLimitSec = 5f; // interval when user can dash;ус
    public float DashMaxSpeed = 12f; // maximum speed on dash 
    public float DashAccelerationPercent = 1f; //  how fast we reach limit when dash pressed

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


    //Inertia and 
    public float OnWhichSpeedCanRotate = 0f; //we will apply inertia until we hit this floor and only then can rotate 
    public float InertiaStopPercentCoef = 0.33f; // how fast we stop  for inertia

    public float
        SpeedAccelerationPercent = 0.23f; // percent of current normal speed that we apply to actualSpeed to reach limit


    public float ThresholdPercent = 0.15f; //if abs(_actualSpeed - normalSpeed)/Max(_actualSpeed,normalSpeed)<threshold
    // then we assign normal speed to actual
    // must be between 0 and 1


    //CAMERA STUFF
    public Camera camera;


    // could be used from other scripts like rotate on triggers

    //PRIVATE VARS

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
        _currentNormalSpeed = SpeedOnGround;
        _currentActualSpeed = 0f;
        _remainDash = Mathf.Max(DashMaxSpeed - _currentNormalSpeed, 0);
        _characterHeight = _controller.height;
        _initialLocalScale = _tMesh.localScale;
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


        _jSpeed += Gravity * Time.deltaTime * FallSpeed;

        dirVector = (dirVector + Vector3.up * _jSpeed) * Time.deltaTime;

        _controller.Move(dirVector);
        UpdateCameraPosition();
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
        if (!direction)
        {
            // Rotate
            if (_currentActualSpeed <= OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
                direction = !direction;
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
        if (direction)
        {
            if (_currentActualSpeed <= OnWhichSpeedCanRotate)
            {
                transform.rotation *= Quaternion.Euler(0, 180f, 0);
                direction = !direction; //TODO what the fuck
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
                _currentActualSpeed += DashAccelerationPercent * _remainDash * Time.deltaTime;
            }
            else
            {
                _currentActualSpeed += SpeedAccelerationPercent * _currentNormalSpeed * Time.deltaTime;
            }
        }
        else if (_currentActualSpeed > _currentNormalSpeed)
        {
            _currentActualSpeed -= SpeedAccelerationPercent * _currentNormalSpeed * Time.deltaTime;
        }

        float percent = Math.Abs(_currentActualSpeed - _currentNormalSpeed) /
                        Math.Max(_currentNormalSpeed, _currentActualSpeed);
        if (percent < ThresholdPercent)
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
            Math.Max(0f, _currentActualSpeed - _currentNormalSpeed * InertiaStopPercentCoef);
    }

    private void UpdateCameraPosition()
    {
        //UPDATED_BY_GD
        camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y,
            gameObject.transform.position.z - 9);
    }

    private void Jump()
    {
        _jSpeed += JumpSpeed;
        //dirVector = transform.forward * _currentActualSpeed;
    }

    /// <summary>
    /// Update _currentDashTime,_currentNormalSpeed according to DashLimitSec when user press Dash button
    /// </summary>
    private void DashPressed()
    {
        if (_currentDashTime < DashLimitSec)
        {
            _currentDashTime += Time.deltaTime;
            _currentNormalSpeed = DashMaxSpeed;
        }
        else
        {
            _currentNormalSpeed = SpeedOnGround;
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
            _currentNormalSpeed = SpeedOnGround;
        }


        if (!charOnTheGround)
        {
            _currentNormalSpeed = SpeedInAir;
        }

        if (isCrouch)
        {
            _currentNormalSpeed = CrouchSpeed;
        }
    }

    /// <summary>
    /// Update controller properties and _current Normal speed based on isCrouchStatus when button pressed
    /// </summary>
    private void CrouchPressed()
    {
        if (!isCrouch && IsOnTheGround())
        {
            _tMesh.localScale = new Vector3(_initialLocalScale.x, LocalScaleY, _initialLocalScale.z);
            _controller.height = ControllerHeight;
            _currentNormalSpeed = CrouchSpeed;
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
        _currentNormalSpeed = SpeedOnGround;
    }
}