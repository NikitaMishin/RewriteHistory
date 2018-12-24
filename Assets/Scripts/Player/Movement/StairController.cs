using System.Collections;
using System.Collections.Generic;
using ReverseTime;
using UnityEditor.Rendering.PostProcessing;
using UnityEngine;

public class StairController : OrdinaryPlayerController
{
    [SerializeField] private float _stairSpeed = 10;


    void Awake()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _managerController = GetComponent<ManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_managerController.ShouldRewind()) return;

        dirVector = Vector3.zero;

        InitialSpeedSetup();

        if (Input.GetKey(KeyCode.D))
        {
            PressRightMove();
        }

        if (Input.GetKey(KeyCode.A))
        {
            PressLeftMove();
        }

        dirVector *= Time.deltaTime;

        if (Input.GetKey(KeyCode.W))
        {
            dirVector += Vector3.up * _stairSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            dirVector += -Vector3.up * _stairSpeed * Time.deltaTime;
        }

        _controller.Move(dirVector);
    }

    public new void RecordTimePoint()
    {
        _managerController.TimePoints.AddLast(new StairPlayerControllerTimePoint(transform.position,
            transform.rotation));
    }

    public new void StartRewind()
    {
        if (_managerController.TimePoints.Count <= 0)
        {
            return;
        }

        var timePoint = (StairPlayerControllerTimePoint) _managerController.TimePoints.Last.Value;
        transform.position = timePoint.Position;
        transform.rotation = timePoint.Rotation;
        _managerController.TimePoints.RemoveLast();
    }
}