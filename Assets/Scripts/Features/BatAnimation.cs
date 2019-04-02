using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAnimation : MonoBehaviour {

    [SerializeField] private BezierCurveMovementWithRewind bezierCurveMovementWithRewind;

    private Animator[] _betAnimators;

    private bool _wasEnabled = false;

    private void Start()
    {
        _betAnimators = gameObject.GetComponentsInChildren<Animator>();
    }

    // Update is called once per frame
    void Update () {
		if (gameObject.transform.position.y > 0.3f && !_wasEnabled)
        {
            _wasEnabled = true;
            for (int i = 0; i < _betAnimators.Length; i++)
            {
                _betAnimators[i].enabled = true;
            }
        } else if (gameObject.transform.position.y < 0.3 && _wasEnabled)
        {
            _wasEnabled = false;
            for (int i = 0; i < _betAnimators.Length; i++)
            {
                _betAnimators[i].enabled = false;
            }
        }
	}
}
