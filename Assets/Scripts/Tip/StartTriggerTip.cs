using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTriggerTip : MonoBehaviour {

    [SerializeField] private string text;
    [SerializeField] private float time;
    [SerializeField] private float timeToClose;

    private Tip _tip;

    private float _startTime = 0;
    private bool _needToShow;

    private void Start()
    {
        _tip = FindObjectOfType<Tip>();
        CloseTip();
    }

    private void OnTriggerEnter(Collider other)
    {
        _needToShow = true;
        _startTime = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        _needToShow = false;

   
   //     Invoke("CloseTip", timeToClose);
    }

    private void OnTriggerStay(Collider other)
    {
        if (_needToShow && Time.time - _startTime > time)
        {
            _needToShow = false;
            _tip.SetVisible(true);
            _tip.SetText(text);
            Invoke("CloseTip", timeToClose);
        }
    }

    private void CloseTip()
    {
        _tip.SetVisible(false);
    }

}
