using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointController : MonoBehaviour {

    [SerializeField] private float timeToReturn;

    private CheckPointTrigger _currentTrigger;
    private ManagerStates _managerStates;
    private ManagerController _managerController;
    private float _timeStart = 0;

	// Use this for initialization
	void Start () {
        _managerStates = gameObject.GetComponent<ManagerStates>();
        _managerController = gameObject.GetComponent<ManagerController>();
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

        if (Time.time - _timeStart > timeToReturn && _currentTrigger != null)
        {
            gameObject.transform.position = _currentTrigger.transform.position;
            _managerStates.ChangeState(State.Default);
            Messenger.Broadcast(GameEventTypes.DEAD);
        }

    }

    public void SetTrigger(CheckPointTrigger trigger)
    {
        _currentTrigger = trigger;
    }

    public CheckPointTrigger GetTrigger()
    {
        return _currentTrigger;
    }

   
}
