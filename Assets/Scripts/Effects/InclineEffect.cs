using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineEffect : MonoBehaviour {

    [SerializeField] private ParticleSystem _effect;

    private ManagerController _managerController;

	// Use this for initialization
	void Start () {
        _managerController = GetComponent<ManagerController>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_managerController.IsOnTheIncline && _managerController.forceVector.magnitude > Vector3.one.magnitude)
        {
            _effect.Play();
        }
        else
        {
            _effect.Stop();
        }
	}
}
