using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

    [SerializeField]
    private GameObject _respawn;

    private InteractSignal _interactSignal;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void Awake()
    {
        _interactSignal = gameObject.GetComponent<InteractSignal>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Trap"))
        {
            GoToRespawn();
        }
    }

    public void GoToRespawn()
    {
        _interactSignal.InterruptInteract();

        gameObject.transform.position = new Vector3(
            _respawn.transform.position.x,
            _respawn.transform.position.y + 2,
            _respawn.transform.position.z
        );
    }
}
