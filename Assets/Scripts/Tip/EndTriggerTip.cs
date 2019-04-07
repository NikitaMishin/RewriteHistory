using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTriggerTip : MonoBehaviour {


    [SerializeField] private bool onlyOneTime = false;

    private bool _wasClosed = false;
    private Tip _tip;

    private void Start()
    {
        _tip = FindObjectOfType<Tip>();
        CloseTip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (onlyOneTime && _wasClosed)
            return;

        CloseTip();
        _wasClosed = true;
    }

    private void CloseTip()
    {
        _tip.SetVisible(false);
    }

}
