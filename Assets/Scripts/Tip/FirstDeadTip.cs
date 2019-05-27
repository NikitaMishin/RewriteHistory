using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstDeadTip : DeadTip {
    
    public bool _wasOpen = false;
    

    void Start()
    {
        _managerStates = FindObjectOfType<ManagerStates>();
        _tip = FindObjectOfType<Tip>();
        _managerController = FindObjectOfType<ManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_wasShown && !_managerController.ShouldRewind()
            && !(_managerStates.GetCurrentState() == State.Dead))
        {
            _wasOpen = true;
            _tip.SetVisible(false);
        }

        if (_wasShown)
            return;

        if (_managerStates.GetCurrentState() == State.Dead)
        {
            if (!_wasShown)
            {
                _wasShown = true;
                OpenTip();
            }
        }
    }
}
