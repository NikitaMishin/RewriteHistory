using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushObjectController : MonoBehaviour {

    [SerializeField]
    private float _force = 5f;
    [SerializeField]
    private LayerMask _layer;


    private Vector3 _direction;
    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 0.5f, _layer, QueryTriggerInteraction.Ignore))
        {
            _direction = transform.forward * _force;

            if (Input.GetKey(KeyCode.LeftShift))
                _direction *= 2;

            hit.collider.gameObject.transform.position += _direction * Time.deltaTime;
        }

    }
}
