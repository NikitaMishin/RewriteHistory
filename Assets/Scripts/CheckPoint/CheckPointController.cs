using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {

    [SerializeField] private float timeToReturn;

    private List<CheckPointTrigger> _checkPointTriggers;
    private CheckPointTrigger _currentTrigger;
    private ManagerStates _managerStates;
    private ManagerController _managerController;
    private BezierCurvePlayerController _curveController;
    private int _cureveId = 0;
    private float _timeStart = 0;
    private Vector3 _position;
    private Quaternion _rotation;
    public bool _direction;
    public float _distance;

    public bool isOrdinary;

	// Use this for initialization
	void Start () {
        _managerStates = gameObject.GetComponent<ManagerStates>();
        _managerController = gameObject.GetComponent<ManagerController>();
        _checkPointTriggers = new List<CheckPointTrigger>();
        _curveController = GetComponent<BezierCurvePlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
        if (!(_managerStates.GetCurrentState() == State.Dead) || _managerController.ShouldRewind())
        {
            _timeStart = 0;
            return;
        }

        if (_timeStart == 0)
            _timeStart = Time.time;

        if (Input.GetKey(KeyCode.Q) && _currentTrigger != null)
        {
            _curveController.ReachDistance = _distance;
            gameObject.transform.position = _position;

            gameObject.transform.rotation = _rotation;

            _curveController.CurrentWayPointId = _cureveId;
            _managerStates.ChangeState(State.Default);
       /*     try
            {
                Messenger.Broadcast(GameEventTypes.DEAD);
            }
            catch (Exception e)
            {

            }*/
        }

    }

    public bool SetTrigger(CheckPointTrigger trigger, Vector3 position, Quaternion rotation)
    {
        bool result = false;

        if (!_checkPointTriggers.Contains(trigger))
        {
            _distance = _curveController.ReachDistance;
            _rotation = rotation;
            _position = position;
            _direction = _managerController.direction;
            _cureveId = _curveController.CurrentWayPointId;
            result = true;
            isOrdinary = _managerController.IsOrdinary();
            _currentTrigger = trigger;
            _checkPointTriggers.Add(trigger);
        }

        return result;
    }

    public CheckPointTrigger GetTrigger()
    {
        return _currentTrigger;
    }

   
}
