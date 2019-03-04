using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {

    [FMODUnity.EventRef]
    public string inputsound;
    bool playerismoving = true;
    public float currentSpeed = 6f;
    
    void CallFootsteps()
    {
        if (playerismoving) FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }
	void Start () {
        //currentSpeed = GameObject.FindGameObjectsWithTag("Player").GetComponent<
        InvokeRepeating("CallFootsteps", 0, currentSpeed);
	}
	
	
}
