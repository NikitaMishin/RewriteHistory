using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;

public class OrdinaryAndBezierCheckoutTrigger : MonoBehaviour
{
    /*
     * NOTE: direction of Curve determined by direction of point (0,1,2,3 and so on) e.t 01234==positive,4321=negative 
     * WARNING: player must be tagged as "Player"
     * USAGE:
     * Add triggers near start and end of curve by creating empty objects
     * Add box colliders to empty game objects and mark them as triggers
     * Add curve to scripts
     * for first trigger add second as connected
     * for second trigger add first as connected
     * StartFromEndOfCurve: move from end to start of curve or vice versa  when trigger hit
     * DirectionOfMovementWhenLeaveCurve: to correct our forward direction of player when he leave curve
     */


    public BezierCurve BezierPath; //path that player will follow when  triggered 
    public bool StartFromEndOfCurve = false; //from where we continue move when reachCurve
    public OrdinaryAndBezierCheckoutTrigger connectedTrigger;

    private List<Vector3> CurvePoints;
    private bool _isOnCurve = false;
    public Vector3 DirectionOfMovementWhenLeaveCurve;


    // Use this for initialization
    private void Awake()
    {
        CurvePoints = BezierPath.GetAllPointsAlongCurve();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            var player = other.gameObject;
            var ordinaryPlayerController = player.GetComponent<OrdinaryPlayerController>();
            var bezierPlayerController = player.GetComponent<BezierCurvePlayerController>();
            var manager = player.GetComponent<ManagerController>();


            if (_isOnCurve || connectedTrigger.GetIsOnCurve())
            {
                _isOnCurve = false;
                bezierPlayerController.enabled = false;
                connectedTrigger.SetIsOnCurve(false);
                player.transform.rotation = Quaternion.Euler(DirectionOfMovementWhenLeaveCurve);
                manager.SendSignal(Signals.ActivatePlayerController);
                return;
            }

            _isOnCurve = true;
            connectedTrigger.SetIsOnCurve(true);

            ordinaryPlayerController.enabled = false;
            SetupCurveController(bezierPlayerController);

            manager.SendSignal(Signals.ActivateBezierController);
        }
    }

    private void SetupCurveController(BezierCurvePlayerController controller)
    {
        controller.CurvePoints = CurvePoints;

        if (StartFromEndOfCurve)
        {
            controller.directionCurve = false;
            controller.CurrentWayPointId = CurvePoints.Count - 1;
        }
        else
        {
            controller.directionCurve = true;
            controller.CurrentWayPointId = 0;
        }
    }

    public void SetIsOnCurve(bool status)
    {
        _isOnCurve = status;
    }

    public bool GetIsOnCurve()
    {
        return _isOnCurve;
    }
}