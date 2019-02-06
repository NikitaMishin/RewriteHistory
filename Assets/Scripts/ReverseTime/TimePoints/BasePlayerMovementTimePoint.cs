using UnityEngine;

namespace ReverseTime
{
    public class BasePlayerMovementTimePoint
    {

        public Vector3 position;
        public Quaternion rotation;
        
        public float _currentActualSpeed; // actual speed 

        public bool Direction;
        //FOR DASH
        public float _currentDashTime; // in what period we press dash
        public float _remainDash; // delta between maxDash and NormalSpeed


        // FOR CROUCH
        public bool isCrouch;
        public Transform _tMesh; // Player Transform
        public float _characterHeight;
        public Vector3 localScale;
        
        

        // FOR JUMP and gravity
        public bool isReadyToJump;
        public float jumpPressTime; // when Jump button was pressed

        public float jSpeed;
        public State state;
    }
}