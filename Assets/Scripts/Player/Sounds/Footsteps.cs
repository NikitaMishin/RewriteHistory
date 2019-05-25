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
    public float time;
    public float RegulatorTime = 50;

    void Start()
    {
        script = Player.GetComponent<ManagerController>();
       //InvokeRepeating("CallFootsteps", 0, currentSpeed / regulator_of_steps_speed);
        
    }
    void FixedUpdate()
    {
        currentSpeed = script._currentActualSpeed;
       
        if (currentSpeed > 0)
        {
            playerismoving = true;
        }
        else playerismoving = false;

        if (script.IsOnTheGround() && playerismoving && Time.time - time > RegulatorTime) { time = Time.time; CallFootsteps(); }
    }
    void CallFootsteps()
    {
        FMOD.Studio.EventInstance e = FMODUnity.RuntimeManager.CreateInstance(inputsound);
        e.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        e.start();
        e.release();
         //FMODUnity.RuntimeManager.PlayOneShot(inputsound);
    }
}
