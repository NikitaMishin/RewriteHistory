using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierCurveMovement : MonoBehaviour
{
    /*
   * NOTE: direction of Curve determined by direction of point (0,1,2,3 and so on) e.t 01234==positive,4321=negative 
   * USAGE:
   * Add script to object that must move along curved path
   * Add bezierPath to this script
   */

    public BezierCurve Path;
    public int CurrentWayPointId = 0;
    public float Speed = 0.5f;
    public float RotationSpeed = 5f;
    public bool Direction = true;
    
    public float reachDistance = 1.0f; //to smooth

    private List<Vector3> PathPoints;

    

    // Use this for initialization
    void Start()
    {
        PathPoints = new List<Vector3>(Path.resolution * Path.pointCount);
        PathPoints.AddRange(Path.GetAllPointsAlongCurve());
    }


    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(PathPoints[CurrentWayPointId], transform.position);
        transform.position =
            Vector3.MoveTowards(transform.position, PathPoints[CurrentWayPointId], Time.deltaTime * Speed);

        var lookPos = PathPoints[CurrentWayPointId] - transform.position;
        var rotation = Quaternion.LookRotation(lookPos);
        
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * RotationSpeed);

        if (distance <= reachDistance)
        {
            UpdateIndexPoint();
        }

    }
    

    /// <summary>
    /// Assign next point to follow based on  Direction Current poinr and Path.close
    /// </summary>
    private void UpdateIndexPoint()
    {
        //closed path with positive direction and last point
        if (Direction && CurrentWayPointId >= PathPoints.Count - 1 && Path.close)
        {
            CurrentWayPointId = 0;
        }
        else if (Direction && CurrentWayPointId>=PathPoints.Count - 1  && !Path.close)
        {
            CurrentWayPointId--;
            Direction = !Direction;
        }
        else if (!Direction && CurrentWayPointId == 0 && !Path.close)
        {
            CurrentWayPointId++;
            Direction = !Direction;
        } else if (Direction)
        {
            CurrentWayPointId++;
        } else if (!Direction)
        {
            CurrentWayPointId--;
        }
   
    }
}