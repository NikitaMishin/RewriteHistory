namespace ReverseTime
{
    public class BezierCurvePlayerControllerTimePoint: BasePlayerMovementTimePoint ,ITimePoint
    {
        
        public bool directionCurve; // for moving from start to end or end to start
        public int CurrentWayPointId;
    }
}