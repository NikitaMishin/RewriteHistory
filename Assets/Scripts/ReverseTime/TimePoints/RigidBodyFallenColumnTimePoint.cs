using UnityEngine;

namespace ReverseTime
{
    public struct RigidBodyFallenColumnTimePoint : ITimePoint
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public Vector3 AngularVelocity;
        public bool wasStepped;

        public RigidBodyFallenColumnTimePoint(Vector3 pos, Quaternion rot, Vector3 velocity,Vector3 angularVelocity, bool stepped)
        {
            Position = pos;
            Rotation = rot;
            Velocity = velocity;
            AngularVelocity = angularVelocity;
            wasStepped = stepped;
        }
    }
}