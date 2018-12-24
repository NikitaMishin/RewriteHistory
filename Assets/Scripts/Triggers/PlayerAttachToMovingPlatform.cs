using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttachToMovingPlatform : MonoBehaviour
{
    /*
     * USAGE:
     * CREATE EMPTY OBJECT WITH SCALE 1,1,1
     * ADD MOVEMENT SCRIPT TO HIM
     * ADD ACTUAL PLATFORM AS CHILD TO THIS GAME OBJECT
     * ADD THIS TRIGGER TO THIS ACTUAL PLATFORM AND MARK IS TRIGGER ON THIS OBJECT
     * THIS SCRIPT JUST ATTACH USER TO MOVET OBJECT I.E INHERIT ALL ROTATION AND POSITION
     */
    private Transform _platform;

    private void Awake()
    {
        if (transform.parent.gameObject == null || transform.parent.gameObject.transform.localScale != Vector3.one)
        {
            throw new Exception("Actual platform should be parent of empty game object with 1,1,1 scale");
        }

        _platform = transform.parent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = _platform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }
}