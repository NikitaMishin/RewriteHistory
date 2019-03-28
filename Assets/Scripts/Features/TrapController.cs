using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

    [SerializeField]
    private GameObject _respawn;

    private ManagerStates _managerStates;
    

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        _managerStates = gameObject.GetComponent<ManagerStates>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Trap"))
        {
            Dead();
        }
    }

    public void GoToRespawn()
    {
        gameObject.transform.position = new Vector3(
            _respawn.transform.position.x,
            _respawn.transform.position.y + 2,
            _respawn.transform.position.z
        );
    }

    public void Dead()
    {
        _managerStates.ChangeState(State.Dead);
    }
}
