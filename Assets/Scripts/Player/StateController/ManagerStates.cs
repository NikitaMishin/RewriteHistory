using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerStates : MonoBehaviour {

    private ManagerController _managerController;
    private TrapController _trapController;
    private InteractSignal _interactSignal;
    private CharacterController _characterController;
    private BezierCurvePlayerController _bezierCurvePlayerController;

    [SerializeField]
    private State _currentState;
    private bool hasRespawn = false;

    public bool canRewind = false;

    public bool HasRespawn
    {
        get
        {
            return hasRespawn;
        }

        set
        {
            hasRespawn = value;
        }
    }

    private void Start()
    {
        _currentState = State.Default;
        _bezierCurvePlayerController = gameObject.GetComponent<BezierCurvePlayerController>();
        _managerController = gameObject.GetComponent<ManagerController>();
        _trapController = gameObject.GetComponent<TrapController>();
        _interactSignal = gameObject.GetComponent<InteractSignal>();
        _characterController = gameObject.GetComponent<CharacterController>();
        _managerController.Init();
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
        Messenger.Broadcast(GameEventTypes.DEAD);
        _currentState = State.Dead;
        _interactSignal.InterruptInteract();
        canRewind = true;
        if (HasRespawn)
            DeadRespawn();
        else
            SimpleDead();

      
    }

    public void DeadRespawn()
    {
        _trapController.GoToRespawn();
        Default();
    }

    public void SimpleDead()
    {
        _managerController.animator.SetBool("IsDead", true); 
        
     //  _characterController.enabled = false;
     //   gameObject.AddComponent<Rigidbody>();
      //  gameObject.AddComponent<CapsuleCollider>();
      //  Rigidbody rigidbody = gameObject.GetComponent<Rigidbody>();
        //CapsuleCollider capsuleCollider = gameObject.GetComponent<CapsuleCollider>();

//        capsuleCollider.height = _characterController.height;
  //      capsuleCollider.radius = _characterController.radius;
    //    capsuleCollider.center = _characterController.center;
        //rigidbody.AddForceAtPosition(gameObject.transform.forward * 3, capsuleCollider.bounds.max, ForceMode.Force);*/
    }

    public void Default()
    {
        _managerController.animator.SetBool("IsDead", false);


        _currentState = State.Default;
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
