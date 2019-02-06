using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStates : MonoBehaviour {

    private State _currentState;
    private ManagerController _managerController;
    private TrapController _trapController;
    private InteractSignal _interactSignal;
    private CharacterController _characterController;
    

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        _currentState = State.Default;
        _managerController = gameObject.GetComponent<ManagerController>();
        _trapController = gameObject.GetComponent<TrapController>();
        _interactSignal = gameObject.GetComponent<InteractSignal>();
        _characterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update () {
		
	}

    public State GetCurrentState()
    {
        return _currentState;
    }

    public void ChangeState(State newState)
    {
        if (_currentState == newState)
            return;

        switch (newState)
        {
            case State.Dead:
                Dead();
                break;
            case State.Default:
                Default();
                break;
            case State.MoveBox:
                MoveBox();
                break;
        }
    }

    public void Dead()
    {
        _currentState = State.Dead;
        _interactSignal.InterruptInteract();
        _characterController.enabled = false;
        
        gameObject.AddComponent<Rigidbody>();
        gameObject.AddComponent<CapsuleCollider>();
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

        capsuleCollider.height = _characterController.height;
        capsuleCollider.radius = _characterController.radius;
        capsuleCollider.center = _characterController.center;
        rigidbody.AddForceAtPosition(gameObject.transform.forward * 3, capsuleCollider.bounds.max, ForceMode.Force);

      //  _trapController.GoToRespawn();
    }

    public void Default()
    {
        _currentState = State.Default;
        _characterController.enabled = true;

        DeleteRigidbody();
        DeleteCollider();
        _interactSignal.InterruptInteract();
    }

    public void MoveBox()
    {
        _currentState = State.MoveBox;
        _characterController.enabled = true;

        DeleteRigidbody();
        DeleteCollider();
        _interactSignal.ActivateInteract();
    }

    public void DeleteCollider()
    {
        CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

        if (capsuleCollider != null)
        {
            Destroy(capsuleCollider);
        }
    }

    public void DeleteRigidbody()
    {
        Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();

        if (rigidbody != null)
        {
            Destroy(rigidbody);
        }
    }
    
}
