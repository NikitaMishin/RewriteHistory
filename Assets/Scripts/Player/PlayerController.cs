using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    // dash mean acceleration
    public float DashLimitSec = 100f; // interval when user can dash;ус
    public float DashMaxSpeed = 10f; // maximum speed on dash 

    public float Gravity = -9.81f;
    public float FallSpeed = 1f; // how fast we fall 
    public float SpeedOnGround = 5f; // velocity when on ground
    public float SpeedInAir = 0.4f; // velocity when in air
    public float JumpSpeed = 10f; //height of jump
    public float CrouchSpeed = 3f; // speed when crouch

    public float LocalScaleY; // Y scale of tMesh aka local height when he crouch
    public float ControllerHeight; // Y scale of the character controller when he crouch

    public Camera camera;


    // could be used from other scripts like rotate on triggers
    public bool direction = true; // for moving along axis (need for rotating) true for positive false for negative

    //PRIVATE VARS
    private CharacterController _controller;
    private Vector3 dirVector = Vector3.zero; // direction vector


    private float _currentNormalSpeed; // depends on isGrounded and Crouch status 
    private float _currentActualSpeed; // actual speed 
    private float _jSpeed = 0; // initial y axis speed;


    private bool isCrouch = false;
    private float _currentDashTime = 0f; // in what period we press dash
    private float _remainDash; // delta between maxDash and NormalSpeed


    private Transform _tMesh; // Player Transform
    private float _characterHeight;
    private Vector3 _initialLocalScale;


    void Start()
    {
        _controller = GetComponent<CharacterController>();
        _tMesh = GetComponent<Transform>();
        _currentNormalSpeed = SpeedOnGround;
        _currentActualSpeed = _currentNormalSpeed;
        _remainDash = Mathf.Max(DashMaxSpeed - _currentNormalSpeed, 0);
        _characterHeight = _controller.height;
        _initialLocalScale = _tMesh.localScale;
    }

    void Update()
    {
        bool charOnTheGround = isOnTheGround();
        dirVector = Vector3.zero;

        SpeedSetup();

        if (Input.GetKey(KeyCode.S))
        {
            CrouchPressed();
        }
        else
        {
            CrouchStop();
        }


        if (charOnTheGround && (Input.GetKey(KeyCode.LeftShift)) && !isCrouch)
        {
            DashPressed();
        }
        else
        {
            DashStop();
        }


        if (Input.GetKey(KeyCode.D))
        {
            MoveForward();
        }

        if (Input.GetKey(KeyCode.A))
        {
            MoveBack();
        }

        if (charOnTheGround && Input.GetKey(KeyCode.W))
        {
            Jump();
        }

        _jSpeed += Gravity * Time.deltaTime * FallSpeed;

        dirVector = (dirVector + Vector3.up * _jSpeed) * Time.deltaTime;

        _controller.Move(dirVector);
        UpdateCameraPosition();
    }

    public bool isOnTheGround()
    {
        // maybe add custom detection
        return _controller.isGrounded;
    }

    private void MoveForward()
    {
        if (!direction)
        {
            transform.rotation *= Quaternion.Euler(0, 180f, 0);
            direction = !direction;
        }

        dirVector += transform.forward * _currentActualSpeed;
    }

    private void MoveBack()
    {
        if (direction)
        {
            transform.rotation *= Quaternion.Euler(0, 180f, 0);
            direction = !true;
        }

        dirVector += transform.forward * _currentActualSpeed;
    }

    private void UpdateCameraPosition()
    {
        camera.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 5, -4);
    }

    private void Jump()
    {
        _jSpeed += JumpSpeed;
        dirVector = transform.forward * _currentActualSpeed;
    }

    private void DashPressed()
    {
        if (_currentDashTime < DashLimitSec)
        {
            _currentDashTime += Time.deltaTime;
            _currentActualSpeed += _remainDash * Time.deltaTime;
        }
        else
        {
            DashStop();
        }

        if (_currentActualSpeed > DashMaxSpeed)
        {
            _currentActualSpeed = DashMaxSpeed;
        }
    }

    private void DashStop()
    {
        if (_currentActualSpeed > _currentNormalSpeed)
        {
            // not fully covered we decrease speed
            _currentActualSpeed = Mathf.Max(_currentNormalSpeed, _currentActualSpeed - _remainDash * Time.deltaTime);
        }

        if (_currentActualSpeed <= _currentNormalSpeed)
        {
            //we cover and restore
            _currentActualSpeed = _currentNormalSpeed;
            _currentDashTime = Mathf.Max(0f, _currentDashTime - Time.deltaTime); //player rest             
        }
    }

    private void SpeedSetup()
    {
        bool charOnTheGround = isOnTheGround();
        if (charOnTheGround)
        {
            //if character on the ground acceleration=0
            _jSpeed = 0;
            _currentNormalSpeed = SpeedOnGround;
        }
        else
        {
            _currentNormalSpeed = SpeedInAir;
        }

        _remainDash = Mathf.Max(DashMaxSpeed - _currentNormalSpeed, 0);
    }

    private void CrouchPressed()
    {
        if (isCrouch == false)
        {
            _tMesh.localScale = new Vector3(_initialLocalScale.x, LocalScaleY, _initialLocalScale.z);
            _controller.height = ControllerHeight;
            isCrouch = true;
            _currentNormalSpeed = CrouchSpeed;
        }
    }

    private void CrouchStop()
    {
        if (isCrouch == false)
        {
            return;
        }

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