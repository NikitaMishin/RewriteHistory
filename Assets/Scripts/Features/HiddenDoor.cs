using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenDoor : MonoBehaviour, IAnimation {

    [SerializeField] private float timeToClose = 2f;
    [SerializeField] private bool needToCLose = true;
    [SerializeField] private AnimationRewindController animationRewindController;

    private float _lastTime = 0;

    private Animation _animation;
    private bool _wasOpen = false;
    private bool _wasClose = false;
    private bool _triggerEnd = false;
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
        if (!needToCLose || animationRewindController.ShouldRewind())
            return;

        if (Time.time - _lastTime > timeToClose && _triggerEnd && _wasOpen)
        {
            _lastAnimation = CLOSE;
            _animCount++;
            _animation.Play(_lastAnimation);
            _triggerEnd = false;
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (animationRewindController.ShouldRewind())
            return;

        if (!_wasOpen && other.gameObject.tag.Equals("Player"))
        {
            _lastAnimation = OPEN;
            _animCount++;
            _animation.Play(_lastAnimation);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (animationRewindController.ShouldRewind())
            return;

        _triggerEnd = true;
        _lastTime = Time.time;
    }

    public void OnEndOpen()
    {
        _wasOpen = true;
    }

    public void OnEndClose()
    {
        _wasOpen = false;
    }

    public void SetTime(float time)
    {
        if (time == 0)
        {
         //   _wasOpen = false;
            _animation.Stop();
            _animCount--;

            if (_animCount > 0)
                _lastAnimation = _lastAnimation.Equals(OPEN) ? CLOSE : OPEN;
            else
                _lastAnimation = OPEN;
        }

        if ((int)(time + 0.1f) == 1)
        {
            _animation.Play(_lastAnimation);
        }

        if (time != 0)
        {
            _animation[_lastAnimation].time = time;
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
        throw new System.NotImplementedException();
    }

    public float GetSpeed()
    {
        throw new System.NotImplementedException();
    }
}
