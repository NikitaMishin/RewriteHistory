using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractSignal : MonoBehaviour
{

    /*
     * Have to work together with MoveObjectController
     * Have to be on player
     */

    private OrdinaryPlayerController _ordinaryPlayerController;
    private MoveObjectController _moveObjectController;
    private bool _isInteract;
    private ManagerController _managerController;
    private RaycastHit _hit;

    // Use this for initialization
    void Awake()
    {
        _ordinaryPlayerController = gameObject.GetComponent<OrdinaryPlayerController>();
        _moveObjectController = gameObject.GetComponent<MoveObjectController>();
        _managerController = gameObject.GetComponent<ManagerController>();
        _isInteract = false;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;

        if (Input.GetKeyUp(KeyCode.F))
        {

            if (_isInteract)
                InterruptInteract();
            else if (
                Physics.Raycast(transform.position, transform.forward, out hit, 1f) &&
                hit.collider.gameObject.tag.Equals("MovementObject")
                )
            {
                _hit = hit;
                ActivateInteract();
            }
        }
    }

    public void InterruptInteract()
    {
        _isInteract = false;
        _moveObjectController.SetInteractCollider(null);
        Rigidbody rigidbody = _hit.transform.gameObject.GetComponent<Rigidbody>();
        Vector3 vector = rigidbody.velocity;
        vector.x = 0;
        rigidbody.velocity = vector;
        
      //  _hit.collider.gameObject.transform.parent = null;
        _managerController.SendSignal(Signals.ActivatePlayerController);
    }

    public void ActivateInteract()
    {
        _isInteract = true;
        _moveObjectController.SetInteractCollider(_hit.collider);
      //  _hit.collider.gameObject.transform.parent = gameObject.transform;
        _managerController.SendSignal(Signals.ActivateMoveObjectController);
    }

}