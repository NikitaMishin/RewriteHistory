using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour {

    [SerializeField]
    private GameObject _respawn;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Trap"))
        {
            GoToRespawn();
        }
    }

    private void GoToRespawn()
    {
        gameObject.transform.position = new Vector3(
            _respawn.transform.position.x,
            _respawn.transform.position.y + 2,
            _respawn.transform.position.z
        );
    }
}
