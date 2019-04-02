using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalancerController : MonoBehaviour {
    [SerializeField] private float maxAngle;

    private int _countOnRightSide = 0;
    private int _countOnLeftSide = 0;
    private Quaternion _rightAngle;
    private Quaternion _leftAngle;
    private Quaternion _normalPosition;
    private GameObject _root;

    private void Awake()
    {
        _rightAngle = Quaternion.Euler(new Vector3(0, 0, -maxAngle));
        _leftAngle = Quaternion.Euler(new Vector3(0, 0, maxAngle));
        _normalPosition = transform.rotation;
        _root = transform.parent.gameObject;
    }

    private void Update()
    {
        if (_countOnLeftSide < _countOnRightSide)
        {
            _root.transform.rotation = Quaternion.Lerp(_root.transform.rotation, _leftAngle, Time.deltaTime);
        }
        else if (_countOnRightSide < _countOnLeftSide)
        {
            _root.transform.rotation = Quaternion.Lerp(_root.transform.rotation, _rightAngle, Time.deltaTime);
        }
        else
        {
            _root.transform.rotation = Quaternion.Lerp(_root.transform.rotation, _normalPosition, Time.deltaTime);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.tag.Equals("Player") && !other.tag.Equals("BalancerObject"))
            return;

        if (other.gameObject.transform.position.x < _root.transform.position.x)
        {
            _countOnLeftSide++;
        }
        else if (other.gameObject.transform.position.x > _root.transform.position.x)
        {
            _countOnRightSide++;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.transform.position.x < _root.transform.position.x)
        {
            _countOnLeftSide--;
        }
        else if (other.gameObject.transform.position.x > _root.transform.position.x)
        {
            _countOnRightSide--;
        }
    }
}
