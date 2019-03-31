using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController {
    void Jump();
    void StopJump();
    void CrouchStart();
    void CrouchStop();
    void Dash();
    void RestartDash();
    void RightMove();
    void LeftMove();
    void StopActualSpeed();
    void Move();
    void RestartDir();
}
