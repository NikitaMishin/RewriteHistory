using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectController : OrdinaryPlayerController
{

    /*
     * Controller for player
     * when he is moving the object with rigidbody
     * 
     * Set controller to the player
     * Disable controller
     * Set InteractSignal to player
     * Set Tag "MovementObject" to necessary object
     * 
     * For activate moving click f
     */

    private Collider _colliderInteract;

    private Rigidbody _rigidbody;
    private Vector3 _offset;
    private RaycastHit _hit;
    private InteractSignal _interactSignal;


    // Use this for initialization
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _tMesh = GetComponent<Transform>();
        _managerController = GetComponent<ManagerController>();
        _managerController._currentNormalSpeed = _managerController.SpeedOnGround;
        _remainDash = Mathf.Max(_managerController.DashMaxSpeed - _managerController._currentNormalSpeed, 0);
        _characterHeight = _controller.height;
        _initialLocalScale = _tMesh.localScale;
        _timeController = FindObjectOfType<TimeController>();
        _interactSignal = gameObject.GetComponent<InteractSignal>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_managerController.ShouldRewind() || _colliderInteract == null) return;

        bool charOnTheGround = IsOnTheGround();

        dirVector = Vector3.zero;

        InitialSpeedSetup();


        if (Input.GetKey(KeyCode.D))
        {
            PressRightMove();
                
        }

        if (Input.GetKey(KeyCode.A))
        {
            PressLeftMove();
        }

        
        _jSpeed += _managerController.Gravity * Time.deltaTime * _managerController.FallSpeed;

        dirVector = (dirVector + Vector3.up * _jSpeed) * Time.deltaTime;

        Vector3 vector = _rigidbody.velocity;
        vector.x = dirVector.x / Time.deltaTime;
        _rigidbody.velocity = vector;
        _controller.Move(dirVector);

        if (Mathf.Abs(_offset.x*2) < Mathf.Abs(_colliderInteract.transform.position.x - gameObject.transform.position.x))
            _interactSignal.InterruptInteract();
    }

    public void SetInteractCollider(Collider collider)
    {
        _colliderInteract = collider;
        SetActualSpeed(0);

        if (_colliderInteract != null)
        {
            _offset = _colliderInteract.transform.position - gameObject.transform.position;
            _rigidbody = _colliderInteract.gameObject.GetComponent<Rigidbody>();
        }
    }



}