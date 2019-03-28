using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableRigidBody : MonoBehaviour {

    [SerializeField] private float seconds = 1;

    private Rigidbody _rigidbody;

	// Use this for initialization
	void Start () {
        _rigidbody = gameObject.GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        InvokeRepeating("Disable", seconds, seconds);
    }

    void Disable()
    {
        Destroy(_rigidbody);
    }
}
