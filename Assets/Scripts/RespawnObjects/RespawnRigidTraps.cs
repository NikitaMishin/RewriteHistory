using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnRigidTraps : MonoBehaviour {

    [SerializeField]
    private GameObject prefab;

    [SerializeField]
    private int respawnSeconds;

    [SerializeField]
    private int lifeSeconds = 10;

	// Use this for initialization
	void Start () {
        InvokeRepeating("Respawn", 0, respawnSeconds);
   //     Respawn();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void Respawn()
    {
        Destroy(Instantiate(prefab, transform.position, transform.rotation), lifeSeconds);
    }
}
