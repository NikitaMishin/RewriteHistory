using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenColumn : MonoBehaviour {

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;
    [SerializeField] private float force = 10f;

    private bool _wasStepped = false;

    // Use this for initialization
    void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && !_wasStepped)
        {
            _wasStepped = true;
            rigidbody.AddForceAtPosition(
                other.gameObject.transform.forward * force,
                new Vector3(
                    collider.bounds.max.x,
                    collider.bounds.max.y,
                    collider.bounds.center.z
                ),
                ForceMode.Impulse
            );
        }
    }

    public bool WasStepped()
    {
        return _wasStepped;
    }

    public void SetWasStepped(bool value)
    {
        _wasStepped = value;
    }
}
