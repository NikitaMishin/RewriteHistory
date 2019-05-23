using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootprintCreator : MonoBehaviour {

    [SerializeField] private GameObject footprintLeft;
    [SerializeField] private GameObject footprintRight;
    [SerializeField] private Transform parent;

    [SerializeField] private Transform positionLeft;
    [SerializeField] private Transform positionRight;

    private ManagerController _managerController;

    private void Start()
    {
        _managerController = gameObject.transform.parent.gameObject.GetComponent<ManagerController>();
    }

    public void DrawLeft()
    {
        if (!_managerController.IsOnTheGround())
            return;

        Vector3 v = parent.rotation.eulerAngles;
        Instantiate(footprintLeft, positionLeft.position, Quaternion.Euler(new Vector3(
            v.x, 
            v.y - 90,
            v.z
            )));
    }

    public void DrawRight()
    {
        if (!_managerController.IsOnTheGround())
            return;

        Vector3 v = parent.rotation.eulerAngles;
        Instantiate(footprintRight, positionRight.position, Quaternion.Euler(new Vector3(
            v.x,
            v.y - 90,
            v.z
            )));
    }
}
