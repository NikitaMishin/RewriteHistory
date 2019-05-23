
using System;
using UnityEngine;

public class SphereRewind : AnimationRewind
{
    [SerializeField] private float rotationSpeed = 1;
    
    private AnimationRewindController _animationRewind;

    private void Start()
    {
        _animation = gameObject.GetComponent<Animation>();
        _animation[nameAnimation].speed = rotationSpeed;
        _animationRewind = GetComponent<AnimationRewindController>();
    }

    private void Update()
    {
        if (_animationRewind.ShouldRewind())
        {
            _animation[nameAnimation].speed = 0;
        }
        else
        {
            _animation[nameAnimation].speed = rotationSpeed;
        }
    }

    private void OnValidate()
    {
        if (_animation != null) 
            _animation[nameAnimation].speed = rotationSpeed;
    }
}
