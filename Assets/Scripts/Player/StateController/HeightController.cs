using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour {

    [SerializeField]
    private float _deadSpeed = -30;

    private OrdinaryPlayerController _ordinaryPlayerController;
    private ManagerStates _managerStates;

	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        _ordinaryPlayerController = gameObject.GetComponent<OrdinaryPlayerController>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }
	
	// Update is called once per frame
	void Update () {
		if (_ordinaryPlayerController.IsOnTheGround())
        {
            if (_ordinaryPlayerController.GetFallSpeed() < _deadSpeed)
            {
                _managerStates.ChangeState(State.Dead);
            }
        }
	}
}
