using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenColumn : MonoBehaviour {

    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private Collider collider;
    [SerializeField] private float force = 10f;
    [SerializeField] private float maxAngle = 45;
    [SerializeField] private ManagerController _managerController;

    private Vector3 _forward = Vector3.zero;

    private GameObject _root;

    private bool _wasStepped = false;

    // Use this for initialization
    void Start()
    {
        _root = gameObject.transform.parent.gameObject;
    }

    private void Update()
    {
      //  if (_wasStepped && Mathf.Abs(_root.transform.rotation.eulerAngles.z) < maxAngle)
        //    _root.transform.rotation = Quaternion.Euler(_root.transform.rotation.eulerAngles + new Vector3(0, 0, force * -1 * Time.deltaTime));
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (_forward.Equals(Vector3.zero))
                _forward = other.gameObject.transform.forward;

            _wasStepped = true;


            if (!_forward.Equals(other.gameObject.transform.forward))
                return;

            rigidbody.AddForceAtPosition(
                other.gameObject.transform.forward * force * Time.deltaTime,
                new Vector3(
                    collider.bounds.max.x,
                    collider.bounds.max.y,
                    collider.bounds.center.z
                ),
                ForceMode.Impulse
            );

            _managerController.forceVector = rigidbody.velocity;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _managerController.forceVector = Vector3.zero;
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
