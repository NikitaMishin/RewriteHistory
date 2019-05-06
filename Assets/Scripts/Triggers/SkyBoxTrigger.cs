using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AzureSky;

public class SkyBoxTrigger : MonoBehaviour
{

    [SerializeField] private float value;
    
    private AzureSkyController _azureSkyController;

    private void Start()
    {
        _azureSkyController = FindObjectOfType<AzureSkyController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(SetValue());
    }

    IEnumerator SetValue()
    {
    
        while (_azureSkyController.timeOfDay.hour < value)
        {
            _azureSkyController.timeOfDay.hour += 0.01f;
            yield return new WaitForSeconds(0);
        }
    }
}
