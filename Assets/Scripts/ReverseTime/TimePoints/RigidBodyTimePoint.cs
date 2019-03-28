using UnityEngine;

namespace ReverseTime
{
    public struct RigidBodyTimePoint : ITimePoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;

        public RigidBodyTimePoint(Vector3 pos, Quaternion rot, Vector3 velocity,Vector3 angularVelocity)
        {
            Position = pos;
            Rotation = rot;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
        }
    }
}