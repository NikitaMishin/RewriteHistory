using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyController : MonoBehaviour {

    private OrdinaryPlayerController _ordinaryPlayerController;
    private BezierCurvePlayerController _bezierCurvePlayerController;
    private MoveObjectController _moveObjectController;
    private ManagerController _managerController;
    private ManagerStates _managerStates;
    private IController _currentController;
    private TimeControllerObject _timeControllerObject;

	// Use this for initialization
	void Start () {
        _ordinaryPlayerController = gameObject.GetComponent<OrdinaryPlayerController>();
        _bezierCurvePlayerController = gameObject.GetComponent<BezierCurvePlayerController>();
        _moveObjectController = gameObject.GetComponent<MoveObjectController>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
        _timeControllerObject = FindObjectOfType<TimeControllerObject>();
        _managerController = gameObject.GetComponent<ManagerController>();
	}
	
    private void SetCurrentController()
    {
        if (_ordinaryPlayerController.enabled)
            _currentController = _ordinaryPlayerController;
        else if (_bezierCurvePlayerController.enabled)
            _currentController = _bezierCurvePlayerController;
        else
            _currentController = _moveObjectController;
    }

    private void SendButtonCommands()
    {
        if (Input.GetKey(KeyCode.S))
        {
            _currentController.CrouchStart();
        }
        else
        {
            _currentController.CrouchStop();
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            _currentController.Dash();
        }
        else
        {
            _currentController.RestartDash();
        }

        if (Input.GetKey(KeyCode.D))
        {
            _currentController.RightMove();
        }

        if (Input.GetKey(KeyCode.A))
        {
            _currentController.LeftMove();
        }

        if (!Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
            _currentController.StopActualSpeed();

        if (Input.GetKeyDown(KeyCode.W))
        {
            _currentController.Jump();
        }
        else
        {
            _currentController.StopJump();
        }
    }

	// Update is called once per frame
	void Update () {
        if (_managerController.ShouldRewind() 
            || _managerStates.GetCurrentState() == State.Dead
            || _timeControllerObject.IsReversing)
            return;

        SetCurrentController();

        _currentController.RestartDir();

        if (!_managerController.onlySlide)
            SendButtonCommands();

        _currentController.Move();
    }
}
