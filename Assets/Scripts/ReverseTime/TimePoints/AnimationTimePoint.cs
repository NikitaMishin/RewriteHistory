using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseTime
{
    public class AnimationTimePoint : ITimePoint
    {
        public float currentTime;
        public float speed;

        public AnimationTimePoint(float time, float speed)
        {
            currentTime = time;
            this.speed = speed;
        }
    }
}