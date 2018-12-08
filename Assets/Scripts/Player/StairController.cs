using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairController : PlayerController {

    [SerializeField]
    private float _stairSpeed = 10;

    // Use this for initialization
    void Start () {
		
	}

    void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {

        dirVector = Vector3.zero;

        InitialSpeedSetup();

        if (Input.GetKey(KeyCode.D))
        {
            PressRightMove();
        }

        if (Input.GetKey(KeyCode.A))
        {
            PressLeftMove();
        }

        dirVector *= Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            dirVector += Vector3.up * _stairSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dirVector += -Vector3.up * _stairSpeed * Time.deltaTime;

        }

        _controller.Move(dirVector);
    }
}
