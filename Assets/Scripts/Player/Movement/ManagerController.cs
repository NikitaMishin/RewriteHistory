using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerController : MonoBehaviour
{
    /*
     * add script to player
     * set constant as u wish
     * 
     */
    // DIRECTION
    public bool direction = true; // for moving along axis (need for rotating) true for positive false for negative


    // DASH
    public float DashLimitSec = 5f; // interval when user can dash;ус
    public float DashMaxSpeed = 12f; // maximum speed on dash 
    public float DashAccelerationPercent = 1f; //  how fast we reach limit when dash pressed

    //GRAVITY
    public float Gravity = -9.81f;
    public float FallSpeed = 1.5f; // how fast we fall 

    //SPEED On GROUND
    public float SpeedOnGround = 6f; // velocity when on ground

    //CROUCH
    public float CrouchSpeed = 3f; // speed when crouch
    public float LocalScaleY; // Y scale of tMesh aka local height when he crouch
    public float ControllerHeight; // Y scale of the character controller when he crouch


    // JUMP and air
    public float JumpSpeed = 8f; //height of jump
    public float SpeedInAir = 4f; // velocity when in air


    //Inertia and 
    public float OnWhichSpeedCanRotate = 0f; //we will apply inertia until we hit this floor and only then can rotate 
    public float InertiaStopPercentCoef = 0.33f; // how fast we stop  for inertia

    public float
        SpeedAccelerationPercent = 0.23f; // percent of current normal speed that we apply to actualSpeed to reach limit


    public float ThresholdPercent = 0.15f; //if abs(_actualSpeed - normalSpeed)/Max(_actualSpeed,normalSpeed)<threshold
    // then we assign normal speed to actual
    // must be between 0 and 1

    public float _currentActualSpeed = 0; // actual speed


    /// <summary>
    /// SIGNAL ACTIVATE scripts for specific movement and action
    /// 
    /// </summary>
    /// <param name="signal"></param>
    public void SendSignal(Signals signal)
    {
        switch (signal)
        {
            case Signals.ActivatePlayerController:
                var playerController = GetComponent<OrdinaryPlayerController>();
                playerController.SetActualSpeed(_currentActualSpeed);
                playerController.enabled = true;
                break;
            case Signals.ActivateBezierController:
                var bezierController = GetComponent<BezierCurvePlayerController>();
                bezierController.SetActualSpeed(_currentActualSpeed);
                bezierController.enabled = true;
                break;
            default: throw new NotImplementedException();
        }
    }

    public void SetActualSpeed(float speed)
    {
        _currentActualSpeed = speed;
    }
}