using UnityEngine;

namespace ReverseTime
{
    public struct BezierCurveObjectTimePoint: ITimePoint 
    {
        public Vector3 position;
        public Quaternion rotation;
        public bool wasStepped;
        public readonly bool curveDir;
        public readonly int curWaypointIndex;
        
        public BezierCurveObjectTimePoint(Vector3 pos,Quaternion rot,bool dir,int index, bool stepped)
        {
            position = pos;
            rotation = rot;
            curveDir = dir;
            curWaypointIndex = index;
            wasStepped = stepped;
        }
    
    }
}