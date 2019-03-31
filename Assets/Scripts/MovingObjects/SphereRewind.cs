using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRewind : AnimationRewind {

    private void Start()
    {
        _animation = gameObject.GetComponent<Animation>();
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!_animation.isPlaying)
                _animation.Play();
        }
    }
}
