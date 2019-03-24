using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidBodyEnableTrigger : MonoBehaviour
{
    /*
 * USAGE:
 * Just add this script  to trigger object
 * staticGameObject should have rigidBody with kinematic  
 * staticGameObject should BE enabled rigidBodyRewind component
     *
     * 
 */
    // Use this for initialization
    public GameObject staticGameObject;
    public bool onEnterDir;
    public bool onExitDir;
    public bool isRewindedOnEnter;
    public bool isRewindedOnExit;
    public int counter = 0;

    void Start()
    {
        if (staticGameObject == null)
        {
            throw new Exception("You forgot about staticGameObject");
        }
        if (staticGameObject.GetComponent<RigidBodyRewind>()==null)
        {
            throw new Exception("You forgot  RigidBodyRewind for staticGameObject");
        }

        if (staticGameObject.GetComponent<Rigidbody>() == null)
        {
            throw new Exception("You forgot  RigidBody for staticGameObject");
        }
        
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        onEnterDir = GetDirection(other.gameObject);
        isRewindedOnEnter = IsRewinded(other.gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        onExitDir = GetDirection(other.gameObject);
        isRewindedOnExit = IsRewinded(other.gameObject);

        if ((onExitDir == onEnterDir) && !isRewindedOnExit && !isRewindedOnEnter)
        {
            if (counter == 0)
            {
                PerformAssignTo(staticGameObject);
            }

            counter++;
        }
        else if (isRewindedOnExit && isRewindedOnEnter && (onExitDir == onEnterDir))
        {
            counter--;
            if (counter == 0)
            {
                PerformDeleteOn(staticGameObject);
            }
        }
        else if (isRewindedOnEnter && !isRewindedOnExit && (onExitDir == onEnterDir))
        {
            counter--;
            if (counter == 0)
            {
                PerformDeleteOn(staticGameObject);
            }
        }

        if (counter == -1)
        {
            throw new NotSupportedException("Counter could not be -1");
        }
    }

    private bool GetDirection(GameObject game)
    {
        return game.GetComponent<ManagerController>().direction;
    }

    private bool IsRewinded(GameObject game)
    {
        return game.GetComponent<TimeController>().IsReversing;
    }

    private void PerformAssignTo(GameObject game)
    {
        game.GetComponent<Rigidbody>().isKinematic = false;
       //game.GetComponent<Rigidbody>().useGravity = true;
        game.AddComponent<RigidBodyRewind>().enabled = true;
    }

    private void PerformDeleteOn(GameObject game)
    {
        
            //game.GetComponent<Rigidbody>().useGravity = false;
            game.GetComponent<Rigidbody>().isKinematic = true;
            game.GetComponent<RigidBodyRewind>().enabled = false;

    }
}