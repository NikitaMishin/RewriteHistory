using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimation {
    void SetTime(float time);
    void SetSpeed(float speed);
    float GetTime();
    float GetSpeed();
}
