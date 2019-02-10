﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineTrigger : MonoBehaviour {

    private ManagerController _managerController;

    private Transform _root;
    private bool isExit = false;

    [SerializeField]
    private float slippery = 2;
    [SerializeField]
    private float speedAfter = 4;
    [SerializeField]
    private float inertia = 0.0005f;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _managerController = player.GetComponent<ManagerController>();
        _root = transform.parent;
    }

    // Update is called once per frame
    void Update () {
       
        if (isExit)
        {
            if (!_managerController.IsOnTheGround())
            {
                _managerController.forceVector = Vector3.Lerp(_managerController.forceVector, Vector3.zero, inertia);
            }
            else
            {
                _managerController.forceVector = Vector3.Lerp(
                _managerController.forceVector,
                Vector3.zero,
                Time.deltaTime * speedAfter);
            }

            isExit = _managerController.forceVector != Vector3.zero;
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;

        if (_root.transform.rotation.z == 0)
            return;

        _managerController.forceVector = Vector3.right * slippery;

        if (_root.transform.rotation.z > 0)
            _managerController.forceVector *= -1;

        _managerController.forceVector += Vector3.up * -slippery;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;

        _managerController.IsOnTheIncline = false;
        isExit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;

        isExit = false;
        _managerController.IsOnTheIncline = true;
        _managerController.forceVector = Vector3.zero;
    }
}
