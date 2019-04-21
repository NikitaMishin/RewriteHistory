using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlattenController : MonoBehaviour {

    private CharacterController _characterController;
    private float _height;
    private Vector3 _vectorUp;
    private Vector3 _vectorDown;
    private TrapController _trapController;
    private ManagerStates _managerStates;

    [SerializeField]
    private float rayLenght = 0.35f;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        _characterController = gameObject.GetComponent<CharacterController>();
        _height = _characterController.height * gameObject.transform.localScale.y - 0.3f;
        _vectorUp = new Vector3(0, _characterController.height * transform.localScale.y - 0.3f, 0);
        _vectorDown = new Vector3(0, -0.3f, 0);

        _trapController = gameObject.GetComponent<TrapController>();
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    // Update is called once per frame
    void Update () {
        RaycastHit hitDown;
        RaycastHit hitUp;

        if (
               _managerStates.GetCurrentState() != State.Dead
               && Physics.Raycast(transform.position, -transform.up, out hitUp, rayLenght)
               && Physics.Raycast(transform.position + _vectorUp, transform.up, out hitDown, rayLenght)

            )
        {
            _managerStates.ChangeState(State.Dead);
        }

    }
}
