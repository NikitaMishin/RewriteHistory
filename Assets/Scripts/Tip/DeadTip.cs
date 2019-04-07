using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTip : MonoBehaviour {

    [SerializeField]
    private string text;

    private ManagerStates _managerStates;
    private Tip _tip;
    private bool _wasShown = false;
    private bool _wasClosed = true;

	// Use this for initialization
	void Start () {
        _managerStates = FindObjectOfType<ManagerStates>();
        _tip = FindObjectOfType<Tip>();
    }
	
	// Update is called once per frame
	void Update () {
		if (_managerStates.GetCurrentState() == State.Dead)
        {
            if (!_wasShown)
            {
                _wasShown = true;
                _wasClosed = false;
                _tip.SetText(text);
                _tip.SetVisible(true);
            }
        }
        else if (!_wasClosed)
        {
            _wasShown = false;
            _wasClosed = true;
            _tip.SetVisible(false);
        }
	}
}
