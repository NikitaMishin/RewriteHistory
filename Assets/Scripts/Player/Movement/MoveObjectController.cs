using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectController : OrdinaryPlayerController, IController
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

    private bool _push = false;
    private bool _right = false;


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
        _timeController = FindObjectOfType<TimeControllerPlayer>();
        _interactSignal = gameObject.GetComponent<InteractSignal>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_managerController.ShouldRewind())
            return;

        charOnTheGround = IsOnTheGround();

        if (_colliderInteract == null
            || Mathf.Abs(_offset.x * 1.2f) < Mathf.Abs(_colliderInteract.transform.position.x - gameObject.transform.position.x)
            || _rigidbody.velocity.y < -2f
            || _managerController.jSpeed < -2f
          )
        {
            _managerStates.ChangeState(State.Default);
            return;
        }
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

    void IController.Jump()
    {

    }

    public void CrouchStart()
    {

    }

    void IController.CrouchStop()
    {

    }

    public void Dash()
    {

    }

    public void RestartDash()
    {

    }

    public void RightMove()
    {
        if (_colliderInteract == null)
            return;

        PressRightMove();

        _right = true;

        if (transform.position.x < _colliderInteract.transform.position.x)
            _push = true;
        else
            _push = false;
    }

    public void LeftMove()
    {
        if (_colliderInteract == null)
            return;

        PressLeftMove();

        _right = false;

        if (transform.position.x < _colliderInteract.transform.position.x)
            _push = false;
        else
            _push = true;
    }

    public void StopJump()
    {

    }

    public void Move()
    {
        if (_colliderInteract == null)
            return;

        RaycastHit hit;

        Vector3 startPosition = (_push ? (_right ? new Vector3(_colliderInteract.bounds.max.x, _colliderInteract.bounds.min.y, _colliderInteract.bounds.min.z)
            : _colliderInteract.bounds.min) : transform.position);
        if (
            Physics.Raycast(
                startPosition, transform.forward, out hit, _push ? 0.1f : 0.3f)
                && !hit.transform.gameObject.Equals(_colliderInteract.gameObject)
        )
        {
            Debug.Log(hit.transform.gameObject.tag);
            return;
        }

        _managerController.jSpeed += _managerController.Gravity * Time.deltaTime * _managerController.FallSpeed;

        dirVector = (dirVector + Vector3.up * _managerController.jSpeed + _managerController.forceVector) * Time.deltaTime;

        Vector3 vector = _rigidbody.velocity;
        vector.x = dirVector.x / Time.deltaTime;
        dirVector.x = vector.x * Time.deltaTime;

        _colliderInteract.transform.position = _colliderInteract.transform.position + dirVector;
        _controller.Move(dirVector);
    }



}