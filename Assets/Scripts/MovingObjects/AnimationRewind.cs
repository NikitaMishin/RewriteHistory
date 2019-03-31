using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationRewind : MonoBehaviour, IAnimation {

    [SerializeField] protected ManagerController _managerController;
    [SerializeField] protected string nameAnimation;

    protected bool _wasStepped = false;
    protected float _stoppedTime = 0;
    protected Animation _animation;

    protected float _lastTime = 0;
    protected bool _timeToRun = false;

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
            _animation.Play(nameAnimation);
        }

        if (time != 0)
        {
            _animation[nameAnimation].time = time;
        }
    }

    public float GetTime()
    {
        if (_wasStepped && _animation[nameAnimation].time == 0)
            return _animation[nameAnimation].length - 0.1f;

        return _animation[nameAnimation].time;
    }

    public void SetSpeed(float speed)
    {
        _animation[nameAnimation].speed = speed;
    }

    public float GetSpeed()
    {
        return _animation[nameAnimation].speed;
    }
}
