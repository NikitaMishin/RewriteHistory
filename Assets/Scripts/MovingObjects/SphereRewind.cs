using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereRewind : AnimationRewind
{
    [SerializeField] private float rotationSpeed = 1;


    private void Start()
    {
        _animation = gameObject.GetComponent<Animation>();
        _animation[nameAnimation].speed = rotationSpeed;
    }

    private void OnValidate()
    {
        if (_animation != null) 
            _animation[nameAnimation].speed = rotationSpeed;
    }


    /*   private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            if (!_animation.isPlaying)
                _animation.Play();
        }
    }*/
}
