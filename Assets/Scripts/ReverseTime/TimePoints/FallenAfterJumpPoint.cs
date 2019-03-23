using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReverseTime
{
    public class FallenAfterJumpPoint : ITimePoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public int currentCount;

        public FallenAfterJumpPoint(Vector3 pos, Quaternion rot, Vector3 velocity, Vector3 angularVelocity, int count)
        {
            Position = pos;
            Rotation = rot;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            currentCount = count;
        }
    }
}
