using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InclineTrigger : MonoBehaviour {

    private ManagerController _managerController;
    private Transform _root;
    private bool isExit = false;

    [SerializeField]
    private float slippery = 2;
    [SerializeField]
    private float speedAfter = 4;

	// Use this for initialization
	void Start () {
		
	}

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _managerController = player.GetComponent<ManagerController>();
        _root = transform.parent;
    }

    // Update is called once per frame
    void Update () {
        

        if (isExit)
        {
            if (_managerController._currentActualSpeed != 0)
            {
                _managerController.forceVector = Vector3.zero;
            }

            _managerController.forceVector = Vector3.Lerp(
                _managerController.forceVector,
                Vector3.zero,
                Time.deltaTime * speedAfter);

            isExit = !(_managerController.forceVector == Vector3.zero);
        }
	}

    private void OnTriggerStay(Collider other)
    {
        if (_root.transform.rotation.z == 0)
            return;

        _managerController.forceVector = Vector3.right * slippery;

        if (_root.transform.rotation.z > 0)
            _managerController.forceVector *= -1;

        _managerController.forceVector += Vector3.up * -slippery;
    }

    private void OnTriggerExit(Collider other)
    {
        isExit = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        isExit = false;
        _managerController.forceVector = Vector3.zero;
    }
}
