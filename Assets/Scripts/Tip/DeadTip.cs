using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTip : MonoBehaviour {

    [SerializeField]
    protected string text;
    [SerializeField]
    private float timeAfterDead = 0f;

    protected ManagerStates _managerStates;
    protected ManagerController _managerController;
    protected Tip _tip;
    protected bool _wasShown = false;
    protected bool _wasClosed = true;

    private FirstDeadTip _firstDeadTip;

	// Use this for initialization
	void Start () {
        _managerStates = FindObjectOfType<ManagerStates>();
        _tip = FindObjectOfType<Tip>();
        _managerController = FindObjectOfType<ManagerController>();
        _firstDeadTip = FindObjectOfType<FirstDeadTip>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_firstDeadTip != null && !_firstDeadTip._wasOpen)
            return;


        if (!_managerController.ShouldRewind()
            && !(_managerStates.GetCurrentState() == State.Dead))
            _tip.SetVisible(false);


        if (_managerStates.GetCurrentState() == State.Dead)
        {
            if (!_wasShown)
            {
                _wasShown = true;
                _wasClosed = false;
                Invoke("OpenTip", timeAfterDead);
            }
        }
        else if (!_wasClosed)
        {
            _wasShown = false;
            _wasClosed = true;
        }
	}

    protected void OpenTip()
    {
        if (_managerStates.GetCurrentState() == State.Dead) { 
            _tip.SetText(text);
            _tip.SetVisible(true);
        }
    }
}
