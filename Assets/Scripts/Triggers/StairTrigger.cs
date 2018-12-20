using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour {   

    private GameObject player;
    private OrdinaryPlayerController playerController;
    private StairController stairController;
    private ManagerController _managerController;
    // Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<OrdinaryPlayerController>();
        stairController = player.GetComponent<StairController>();
        _managerController = player.GetComponent<ManagerController>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        playerController.enabled = false;
        _managerController.SendSignal(Signals.ActivateStairsController);
        //stairController.SetManagerController(playerController.GetManagerController());
    }

    private void OnTriggerExit(Collider other)
    {
        playerController.enabled = true;
        _managerController.SendSignal(Signals.ActivatePlayerController);

        //playerController.SetManagerController(stairController.GetManagerController());
    }
}
