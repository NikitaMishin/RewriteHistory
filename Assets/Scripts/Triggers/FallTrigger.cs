using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {

    [SerializeField]
    private float _secondsBeforeFall = 0.5f;

    private Rigidbody _rigidBody;
    private float _startTime;

    // Use this for initialization
    void Start () {
        _rigidBody = gameObject.transform.parent.GetComponent<Rigidbody>();	
	}

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player") && !other.gameObject.tag.Equals("FallenObject"))
            return;

        _startTime = Time.time;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player") && !other.gameObject.tag.Equals("FallenObject"))
            return;

        if (Time.time - _startTime > _secondsBeforeFall)
            _rigidBody.useGravity = true;
    }
}
