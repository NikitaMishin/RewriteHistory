using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenColumn : MonoBehaviour, IAnimation {

    [SerializeField] private ManagerController _managerController;
    [SerializeField] private float timeAfterFallNext = 0.2f;

    private bool _wasStepped = false;
    private float _stoppedTime = 0;
    private Animation _animation;

    private float _lastTime = 0;
    private bool _timeToRun = false;

    private void Start()
    {
        _animation = gameObject.transform.parent.gameObject.GetComponent<Animation>();
    }

    private void Update()
    {
        if (Time.time - _lastTime > timeAfterFallNext && _timeToRun)
        {
            _animation["FallenColumn"].speed = 1;
            _timeToRun = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player") && (!_wasStepped || _animation["FallenColumn"].speed != 0))
        {
            _wasStepped = true;
            _animation.Play();
            _animation["FallenColumn"].speed = 1;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
        {
            //_stoppedTime = _animation["FallenColumn"].time;
          //  _animation["FallenColumn"].speed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        /*if (!other.gameObject.tag.Equals("Player"))
        {
            _lastTime = Time.time;
            _timeToRun = true;
        }*/
    }

    public bool WasStepped()
    {
        return _wasStepped;
    }

    public void SetWasStepped(bool value)
    {
        _wasStepped = value;
    }

    public void SetTime(float time)
    {
        if (time == 0)
        {
            _animation.Stop();
            _wasStepped = false;
        }

        if ((int)(time + 0.1f) == 1)
        {
            _animation.Play("FallenColumn");
        }

        if (time != 0)
        {
            _animation["FallenColumn"].time = time;
        }
    }

    public float GetTime()
    {
        if (_wasStepped && _animation["FallenColumn"].time == 0)
            return _animation["FallenColumn"].length - 0.1f;

        return _animation["FallenColumn"].time;
    }

    public void SetSpeed(float speed)
    {
        _animation["FallenColumn"].speed = speed;
    }

    public float GetSpeed()
    {
        return _animation["FallenColumn"].speed;
    }
}
