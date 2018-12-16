using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairTrigger : MonoBehaviour {   

    private GameObject player;
    private OrdinaryPlayerController playerController;
    private StairController stairController;

    // Use this for initialization
	void Start () {
		
	}

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<OrdinaryPlayerController>();
        stairController = player.GetComponent<StairController>();
    }

    // Update is called once per frame
    void Update () {
       
    }

    private void OnTriggerEnter(Collider other)
    {
        playerController.enabled = false;
        stairController.enabled = true;

        stairController.SetManagerController(playerController.GetManagerController());
    }

    private void OnTriggerExit(Collider other)
    {
        playerController.enabled = true;
        stairController.enabled = false;

        playerController.SetManagerController(stairController.GetManagerController());
    }
}
