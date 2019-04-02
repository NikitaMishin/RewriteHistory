using UnityEngine;
using UnityEngine.Animations;

namespace ReverseTime
{
    /// <summary>
    /// For objects that move with one speed and doesn't create difficult action
    /// Just move and rotate
    /// </summary>
    public struct SimpleTimePoint : ITimePoint
    {
        
        public Vector3 position;
        public Quaternion rotation;
        
        public SimpleTimePoint(Vector3 pos,Quaternion rot)
        {
            position = pos;
            rotation = rot;
        }
    }
}