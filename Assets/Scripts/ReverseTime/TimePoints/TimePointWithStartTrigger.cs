using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseTime
{

    public struct TimePointWithStartTrigger : ITimePoint
    {

        public Vector3 position;
        public Quaternion rotation;
        public bool wasStepped;

        public TimePointWithStartTrigger(Vector3 pos, Quaternion rot, bool stepped)
        {
            position = pos;
            rotation = rot;
            wasStepped = stepped;
        }
    }

}
