using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractAsRigidBody : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag.Equals("Balancer"))
        {
            Rigidbody rigidbody = hit.gameObject.GetComponent<Rigidbody>();
            rigidbody.AddForceAtPosition(transform.up * -5, hit.point, ForceMode.Force);
        }
    }
}
