using ReverseTime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAfterJumpRewind : MonoBehaviour, IRevertListener
{

    /*
	 * USAGE:
	 * Just add this script to  object with rigidBody that will be reversed when user press rewind 
	 * Take  assumption that object have rigidBody
	 *  space complexity = fixedUpdatePerSec * TimeWindow * 13float
	 * 1523kbyte for 300seconds  with 50fixedUpdate per sec and float=8byte
	 */
    // Use this for initialization
    private FallenAfterJumpBlock _fallenAfterJumpBlock;
    private FallenAfterJumpController _fallenAfterJumpController;

    private LinkedList<FallenAfterJumpPoint> _timePoints;
    private FallenAfterJumpPoint _checkPoint;
    private TimeControllerPlayer _timeController;
    private Rigidbody _rb;
    private Collider _collider;
    private ManagerStates _managerStates;

    void Start()
    {
        _fallenAfterJumpBlock = gameObject.GetComponent<FallenAfterJumpBlock>();
        _fallenAfterJumpController = gameObject.transform.parent.gameObject.GetComponent<FallenAfterJumpController>();

        _timePoints = new LinkedList<FallenAfterJumpPoint>();
        _timeController = FindObjectOfType<TimeControllerPlayer>();
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _managerStates = FindObjectOfType<ManagerStates>();
        Messenger.AddListener(GameEventTypes.CHECKPOINT, SavePosition);
        Messenger.AddListener(GameEventTypes.DEAD, RestartPosition);
    }

    private void SavePosition()
    {
        _checkPoint = new FallenAfterJumpPoint(transform.position, transform.rotation, _rb.velocity, _rb.angularVelocity, _fallenAfterJumpBlock.GetCurrentCount());
    }

    private void RestartPosition()
    {
        transform.position = _checkPoint.Position;
        transform.rotation = _checkPoint.Rotation;
        _rb.velocity = _checkPoint.Velocity;
        _rb.angularVelocity = _checkPoint.AngularVelocity;
        _collider.enabled = false;
      //  _rb.velocity = Vector3.zero;

        _fallenAfterJumpBlock.SetCurrentCount(_checkPoint.currentCount);

        if (_fallenAfterJumpController.WasBroken())
            _fallenAfterJumpController.SetWasBroken(_fallenAfterJumpBlock.IsBroken());
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (ShouldRewind())
        {
            StartRewind();
        }
        else if (_managerStates.GetCurrentState() != State.Dead)
        {
            _collider.enabled = true;
            RecordTimePoint();
        }

        if (_timeController.ShouldRemoveOldRecord())
        {
            DeleteOldRecord();
        }
    }

    public void RecordTimePoint()
    {
        _timePoints.AddLast(new FallenAfterJumpPoint(transform.position, transform.rotation, _rb.velocity, _rb.angularVelocity, _fallenAfterJumpBlock.GetCurrentCount()));
    }

    public void StartRewind()
    {
        if (_timePoints.Count > 0)
        {
            var timePoint = _timePoints.Last.Value;
            transform.position = timePoint.Position;
            transform.rotation = timePoint.Rotation;
            _rb.velocity = timePoint.Velocity;
            _rb.angularVelocity = timePoint.AngularVelocity;
            _timePoints.RemoveLast();
        //    _collider.enabled = false;
            //  _rb.velocity = new Vector3(0, _rb.velocity.y, _rb.velocity.z);
            _rb.velocity = Vector3.zero;

            _fallenAfterJumpBlock.SetCurrentCount(timePoint.currentCount);

            if (_fallenAfterJumpController.WasBroken())
                _fallenAfterJumpController.SetWasBroken(_fallenAfterJumpBlock.IsBroken());

        }
    }

    public void DeleteOldRecord()
    {
        if (_timePoints.Count > 0)
        {
            _timePoints.RemoveFirst();
        }
    }

    public void DeleteAllRecord()
    {
        _timePoints.Clear();
    }

    public bool ShouldRewind()
    {
        return _timeController.IsReversing;
    }
}
