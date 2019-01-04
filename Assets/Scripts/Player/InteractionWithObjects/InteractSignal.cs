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
            {
                _isInteract = false;
                _moveObjectController.SetInteractCollider(null);
                _managerController.SendSignal(Signals.ActivatePlayerController);
            }
            else if (Physics.Raycast(transform.position, transform.forward, out hit, 1f) && 
                hit.collider.gameObject.tag.Equals("MovementObject"))
            {
                _isInteract = true;
                _moveObjectController.SetInteractCollider(hit.collider);
                _managerController.SendSignal(Signals.ActivateMoveObjectController);
            }
        }
    }
}