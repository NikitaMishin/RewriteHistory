using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseTime
{
    public class AnimationTimePoint : ITimePoint
    {
        public float currentTime;

        public AnimationTimePoint(float time)
        {
            currentTime = time;
        }
    }
}