using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Footsteps : MonoBehaviour {
    public GameObject Player;
    private ManagerController script;
    [FMODUnity.EventRef]
    public string inputsound;
    public bool playerismoving = true;
    public float currentSpeed , regulator_of_steps_speed = 2f ;
    void Start()
    {
        script = Player.GetComponent<ManagerController>();
        InvokeRepeating("CallFootsteps", 0.5f, currentSpeed / regulator_of_steps_speed);
    }
    void FixedUpdate()
    {
        currentSpeed = script._currentActualSpeed;
        if (currentSpeed > 0)
        {
            playerismoving = true;
        }
        else playerismoving = false;
    }
    void CallFootsteps()
    {
        if (playerismoving) FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }
}
