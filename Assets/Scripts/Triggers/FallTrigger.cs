using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallTrigger : MonoBehaviour {

    [SerializeField]
    private float _fallSpeed = -5f;

    [SerializeField]
    private float _secondsBeforeFall = 0.5f;

    private GameObject _parent;
    private float _defaultY;
    private float _startTime;
    private float _currentGravity;

    private bool _isFallen;

    // Use this for initialization
    void Start () {
		
	}
	
    void Awake()
    {
        _parent = gameObject.transform.parent.gameObject;
        _defaultY = _parent.transform.position.y;
    }

	// Update is called once per frame
	void Update () {

        if (_isFallen)
        {
            _currentGravity += _fallSpeed * Time.deltaTime;
            _parent.transform.position = _parent.transform.position + Vector3.up * Time.deltaTime * _currentGravity;
        }
	}

    private void OnTriggerEnter(Collider other)
    {
    //    if (other.gameObject.tag.Equals("Player"))
        if (!_isFallen)
            _startTime = Time.time;
    
    }

    private void OnTriggerStay(Collider other)
    {
        if (Time.time - _startTime > _secondsBeforeFall)
            _isFallen = true;
    }
}
