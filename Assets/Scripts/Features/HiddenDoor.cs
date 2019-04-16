using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour {

    [SerializeField] private float timeToClose = 2f;
    [SerializeField] private bool needToCLose = true;
    [SerializeField] private SimpleRewind simpleRewind;
    [SerializeField] private GameObject door;
    [SerializeField] private float speed = 10f;

    private ManagerStates _managerStates;

    private float _lastTime = 0;

    private bool _isActing = false;
    private bool _isOpening = true;

    private Vector3 _startAngle;
    private Vector3 _endAngle;

    private void Start()
    {
        _startAngle = new Vector3(0, 0, 0);
        _endAngle = new Vector3(0, 0, 90);
        _managerStates = FindObjectOfType<ManagerStates>();
    }

    private void Update()
    {
        //if (_managerStates.GetCurrentState() == State.Dead)
        //    return;


        if (!needToCLose || simpleRewind != null
            && simpleRewind.enabled
            && simpleRewind.ShouldRewind())
                return;

        if (!_isActing)
            return;

        if (_isOpening)
        {
            door.transform.rotation = Quaternion.Euler(Vector3.Lerp(door.transform.rotation.eulerAngles, _endAngle, Time.deltaTime * speed));
        }
        else if (Time.time - _lastTime > timeToClose)
        {
            door.transform.rotation = Quaternion.Euler(Vector3.Lerp(door.transform.rotation.eulerAngles, _startAngle, Time.deltaTime * speed));
        }


    }

    private void OnTriggerStay(Collider other)
    {
        if (simpleRewind != null
            && simpleRewind.enabled
            && simpleRewind.ShouldRewind())
            return;

        if (other.gameObject.tag.Equals("Player"))
        {
            _isActing = true;
            _isOpening = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (simpleRewind != null 
            && simpleRewind.enabled
            && simpleRewind.ShouldRewind())
            return;

        _isOpening = false;

        _lastTime = Time.time;
    }
}
