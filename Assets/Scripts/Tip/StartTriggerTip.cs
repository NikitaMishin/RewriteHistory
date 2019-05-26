using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTriggerTip : MonoBehaviour
{

    [SerializeField] private Sprite sprite;
    
    [SerializeField] private string text;
    [SerializeField] private float time;
    [SerializeField] private bool onlyOneTime = false;
  //  [SerializeField] private float timeToClose;

    private Tip _tip;

    private bool _wasShown = false;

    private float _startTime = 0;
    private bool _needToShow;

    private void Start()
    {
        _tip = FindObjectOfType<Tip>();
        CloseTip();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_wasShown && onlyOneTime)
            return;

        _needToShow = true;
        _startTime = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        _needToShow = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_needToShow && Time.time - _startTime > time)
        {
            _needToShow = false;
            _tip.SetVisible(true);
            
            if (sprite != null)
                _tip.SetImage(sprite);
            else
                _tip.SetText(text);
            _wasShown = true;
        }
    }

    private void CloseTip()
    {
        _tip.SetVisible(false);
    }

}
