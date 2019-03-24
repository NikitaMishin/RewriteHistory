using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopFallenColumn : MonoBehaviour {

    private Animation _animation;
    private AnimationRewindController _animationRewindController;

    private void Start()
    {
        _animation = gameObject.transform.parent.parent.gameObject.GetComponent<Animation>();
        _animationRewindController = gameObject.transform.parent.parent.GetComponent<AnimationRewindController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_animationRewindController.ShouldRewind())
            return;

        if (other.gameObject.name.Equals("TriggerForSignal"))
        {
            _animation["FallenColumn"].speed = 0;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_animationRewindController.ShouldRewind())
            return;

        if (other.gameObject.name.Equals("TriggerForSignal"))
        {
            _animation["FallenColumn"].speed = 1;
        }
    }
}
