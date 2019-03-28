using UnityEngine;

namespace ReverseTime
{
    public struct RigidBodyTimePoint : ITimePoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public bool useGravity;

        public RigidBodyTimePoint(Vector3 pos, Quaternion rot, Vector3 velocity,Vector3 angularVelocity, bool gravity)
        {
            Position = pos;
            Rotation = rot;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            useGravity = gravity;
        }
    }
}