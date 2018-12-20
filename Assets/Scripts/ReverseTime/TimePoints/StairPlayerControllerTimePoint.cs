using UnityEngine;

namespace ReverseTime
{
    public class StairPlayerControllerTimePoint:ITimePoint
    {
        public Vector3 Position;
        public Quaternion Rotation;

        public StairPlayerControllerTimePoint(Vector3 pos, Quaternion rot)
        {
            Position = pos;
            Rotation = rot;
        }

    }
}