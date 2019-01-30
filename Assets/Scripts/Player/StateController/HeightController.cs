using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeightController : MonoBehaviour {

    [SerializeField]
    private float _deadSpeed = -30;

    private OrdinaryPlayerController _ordinaryPlayerController;
    private TrapController _trapController;
	// Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        _ordinaryPlayerController = gameObject.GetComponent<OrdinaryPlayerController>();
        _trapController = gameObject.GetComponent<TrapController>();
    }
	
	// Update is called once per frame
	void Update () {
		if (_ordinaryPlayerController.IsOnTheGround())
        {
            if (_ordinaryPlayerController.GetFallSpeed() < _deadSpeed)
            {
                _trapController.GoToRespawn();
            }
        }
	}
}
