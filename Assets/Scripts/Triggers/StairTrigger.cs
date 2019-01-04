using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour {   

    private GameObject player;
    private ManagerController _managerController;
    // Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        _managerController = player.GetComponent<ManagerController>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_managerController.CurrentSignal == Signals.ActivatePlayerController)
            _managerController.SendSignal(Signals.ActivateStairsController);
    }

    private void OnTriggerExit(Collider other)
    {
        if (_managerController.CurrentSignal == Signals.ActivateStairsController)
            _managerController.SendSignal(Signals.ActivatePlayerController);
    }
}
