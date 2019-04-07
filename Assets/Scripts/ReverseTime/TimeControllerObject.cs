using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeControllerObject : TimeControllerPlayer {

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Q) && CouldUseReverse)
        {
            currentTimeReverse = Mathf.Max(currentTimeReverse - Time.deltaTime, 0f);
            IsReversing = true;
        }
        else
        {
            IsReversing = false;
            currentTimeReverse = Mathf.Min(MaxTimeReverse, currentTimeReverse + Time.deltaTime);
        }

        shouldRemoveOldRecord = currentTimeReverse >= MaxTimeReverse;
    }
}
