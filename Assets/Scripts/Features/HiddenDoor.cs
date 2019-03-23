using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour, IAnimation {

    private Animation _animation;
    private bool _wasOpen = false;

    private void Start()
    {
        _animation = gameObject.transform.parent.gameObject.GetComponentInChildren<Animation>();   
    }

    private void OnTriggerStay(Collider other)
    {
        if (!_wasOpen && other.gameObject.tag.Equals("Player"))
        {
            _animation.Play("OpenDoor");
            _wasOpen = true;
        }
    }

    public void SetTime(float time)
    {

        if (time == 0)
        {
            _animation.Stop();
            _wasOpen = false;
        }

        Debug.Log((int)(time + 0.1f));

        if ((int)(time + 0.1f) == 1)
        {
            Debug.Log("L: " + _animation["OpenDoor"].length);
            _animation.Play("OpenDoor");
        }

        if (time != 0)
        {
            _animation["OpenDoor"].time = time;
        }


    }

    public float GetTime()
    {
        if (_wasOpen && _animation["OpenDoor"].time == 0)
            return _animation["OpenDoor"].length - 0.1f;

        return _animation["OpenDoor"].time;
    }
}
