using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour {

    [SerializeField]
    private float _friction = 0.3f;
    [SerializeField]
    private float slopeLimit = 10f;
    [SerializeField]
    private float maxTimeWithoutGround = 0.1f;
    [SerializeField]
    private float maxForce = 3f;

    private float _timeWithouGround = 0;

    private CharacterController _characterController;
    private ManagerController _managerController;
    private Vector3 _hitNormal;
    private bool _isGrounded;
    private bool _prevGround = true;
    private Vector3 groundSlopeDir;
    private float groundSlopeAngle;

    // Use this for initialization
    void Start () {
        _characterController = gameObject.GetComponent<CharacterController>();
        _managerController = gameObject.GetComponent<ManagerController>();
    }
	
	// Update is called once per frame
	void Update () {
        _isGrounded = (Vector3.Angle(Vector3.up, _hitNormal) <= slopeLimit);

        if (_characterController.isGrounded)
            _timeWithouGround = Time.time;

        if (_prevGround && !_characterController.isGrounded)
            _timeWithouGround = Time.time;

        _prevGround = _characterController.isGrounded;

        if (Time.time - _timeWithouGround < maxTimeWithoutGround)
        {
            Vector3 force = _managerController.forceVector + groundSlopeDir;
            force.z = 0;
               /* new Vector3(
                            (1f - _hitNormal.y) * _hitNormal.x * (1f - _friction),
                            (1f - _hitNormal.y) * _hitNormal.y * (1f - _friction) * -1,
                            (1f - _hitNormal.y) * _hitNormal.z * (1f - _friction)
                );*/

            if (
                Mathf.Abs(force.x) < maxForce
                && Mathf.Abs(force.y) < maxForce
                && Mathf.Abs(force.z) < maxForce
            )
            {
                _managerController.forceVector = force;
            }
        } else
        {
            _managerController.forceVector = Vector3.zero;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 temp = Vector3.Cross(hit.normal, Vector3.down);
        groundSlopeDir = Vector3.Cross(temp, hit.normal);
        groundSlopeAngle = Vector3.Angle(hit.normal, Vector3.up);
        _hitNormal = hit.normal;
    }
}
