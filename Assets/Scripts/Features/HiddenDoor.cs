using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour, IAnimation {

    [SerializeField] private float timeToClose = 2f;
    [SerializeField] private bool needToCLose = true;
    [SerializeField] private AnimationRewindController animationRewindController;

    private float _lastTime = 0;
    private float _prevTime = 1f;

    private Animation _animation;
    private bool _wasOpen = false;
    private bool _wasOpened = false;
    private bool _triggerEnd = true;
    private int _animCount = 0;

    private string _lastAnimation;
    private readonly string OPEN = "OpenDoor";
    private readonly string CLOSE = "OpenDoorBack";

    private void Start()
    {
        _lastAnimation = OPEN;
        _animation = gameObject.transform.parent.gameObject.GetComponentInChildren<Animation>();   
    }

    private void Update()
    {
        if (!needToCLose || animationRewindController != null
            && animationRewindController.enabled
            && animationRewindController.ShouldRewind())
                return;

        if (Time.time - _lastTime > timeToClose && _triggerEnd && _wasOpened)
        {
            float time = _animation[_lastAnimation].time;
            _lastAnimation = CLOSE;
            _animCount++;
            _animation.Play(_lastAnimation);
         //   _animation[_lastAnimation].time = 1f - time == 1 ? 0 : 1f - time;
            _triggerEnd = false;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (animationRewindController != null
            && animationRewindController.enabled
            && animationRewindController.ShouldRewind())
            return;

        if (!_wasOpened && other.gameObject.tag.Equals("Player") && _triggerEnd)
        {
            float time = _animation[_lastAnimation].time;
            _lastAnimation = OPEN;

            _animCount++;
            _animation.Play(_lastAnimation);
       //     _animation[_lastAnimation].time = 1f - time == 1 ? 0 : 1f - time;
            _triggerEnd = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (animationRewindController != null 
            && animationRewindController.enabled
            && animationRewindController.ShouldRewind())
            return;

        _triggerEnd = true;
        _lastTime = Time.time;
    }

    public void OnEndOpen()
    {
        _wasOpen = true;
        _wasOpened = true;
    }

    public void OnEndClose()
    {
        _wasOpened = false;
        _triggerEnd = true;
    }

    public void SetTime(float time)
    {
        if (time == 0 && _animCount > 0)
        {
            _animation.Stop();
            _animCount--;
            Reset();
            return;
            
        }
        else if (time - _prevTime > 0.3f && _animCount > 0)
        {
            _animation.Stop();
            _animCount--;

            if (_animCount > 0)
                _lastAnimation = _lastAnimation.Equals(OPEN) ? CLOSE : OPEN;
            else
                Reset();
        }
        else if ((int)(time + 0.1f) == _animation[_lastAnimation].length)
        {
            _animation.Play(_lastAnimation);
        }

        if (time != 0 && _animCount > 0)
        {
            _animation[_lastAnimation].time = time;
            _prevTime = time;
        }
    }

    public float GetTime()
    {
        if (_wasOpen && _animation[_lastAnimation].time == 0)
            return _animation[_lastAnimation].length - 0.1f;


        return _animation[_lastAnimation].time;
    }

    public void SetSpeed(float speed)
    {
        _animation[_lastAnimation].speed = speed;
    }

    public float GetSpeed()
    {
        return _animation[_lastAnimation].speed;
    }

    private void Reset()
    {
        _lastAnimation = OPEN;
        _triggerEnd = false;
        _wasOpen = false;
        _prevTime = 1;
    }
}
