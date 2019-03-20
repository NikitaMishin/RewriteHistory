using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour {

    [SerializeField] private ManagerController _managerController;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        _managerController.animator.SetFloat("MovementSpeed", _managerController._currentActualSpeed);
        _managerController.animator.SetFloat("FallSpeed", _managerController.jSpeed);
	}
}
