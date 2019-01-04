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
     * 
     * For moving press F
     */

    private Collider _colliderInteract;

    private Rigidbody _rigidbody;
    private Vector3 _offset;

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
    }

    // Update is called once per frame
    void Update()
    {
        if (_managerController.ShouldRewind()) return;
        if (_colliderInteract != null)
        {
            _colliderInteract.transform.position = new Vector3(0, 0, -100);

            if (_managerController.ShouldRewind()) return;

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

            _controller.Move(dirVector);

            _colliderInteract.transform.position = gameObject.transform.position + _offset;
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
}