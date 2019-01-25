using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject target;

	private Vector3 _offset;
    private Vector3 _prevPosition;

    [SerializeField]
    private float _cameraSpeedX = 3;
    [SerializeField]
    private float _cameraSpeedY = 3;
    [SerializeField]
    private float _playerSpeedX = 10;
    [SerializeField]
    private float _playerSpeedY = 30;
    [SerializeField]
    private float _maxCameraOffsetX = 3;
    [SerializeField]
    private float _maxCameraOffsetY = 3;
    [SerializeField]
    private float _errorBackX = 0.2f;



    private int _direction = 1;

    private Vector3 _diffTarget = Vector3.zero;
    private Vector3 _diffCamera = Vector3.zero;
    private Vector3 _currentOffset = Vector3.zero;


    // Use this for initialization
    void Start ()
	{
		_offset = transform.position - target.transform.position;
        _prevPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }
	
	// Update is called once per frame
	void LateUpdate ()
	{

        _diffTarget = target.transform.position - _prevPosition;
        _diffCamera = transform.position - target.transform.position;


        _currentOffset = new Vector3(
            GetOffsetX(),
            GetOffsetY(),
            0
        );

        transform.position = target.transform.position + _offset;

        transform.position = new Vector3(
            transform.position.x + _currentOffset.x, 
            transform.position.y + _currentOffset.y,
            transform.position.z + _currentOffset.z);

        _prevPosition = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
    }

    private float GetOffsetX()
    {
        Vector3 camera = new Vector3(_currentOffset.x, 0, 0);

        int direction = _diffTarget.x < 0 ? -1 : 1;

        if (_diffTarget.x != 0)
        {
            
            Vector3 goal = new Vector3(camera.x + (_maxCameraOffsetX-camera.x * direction) * direction, camera.y, camera.z);

            if (Mathf.Abs(_diffTarget.x) > (_playerSpeedX -2) * Time.deltaTime)
                camera = Vector3.Lerp(
                    camera,
                    goal, 
                    Time.deltaTime * _cameraSpeedX);
            else
                camera = Vector3.Lerp(camera, Vector3.zero, Time.deltaTime * _cameraSpeedX);
        }
        else if (Mathf.Abs(_diffCamera.x) > 0)
        {
            camera = Vector3.Lerp(camera, Vector3.zero, Time.deltaTime * _cameraSpeedX);
        }
        return 0;
        return camera.x;
    }

    private float GetOffsetY()
    {
        Vector3 camera = new Vector3(0, _currentOffset.y, 0);
        int direction = _diffTarget.y < 0 ? -1 : 1;

        if (_diffTarget.y != 0)
        {

            Vector3 goal = new Vector3(camera.x, camera.y + _maxCameraOffsetY * direction, camera.z);

            if (Mathf.Abs(_diffTarget.y) > _playerSpeedY * Time.deltaTime && Mathf.Abs(_currentOffset.y) < _maxCameraOffsetY)
                camera = Vector3.Lerp(
                    camera,
                    goal,
                    Time.deltaTime * _cameraSpeedY);
            else if (Mathf.Sign(_diffCamera.y) != Mathf.Sign(_direction))
                camera = Vector3.Lerp(camera, Vector3.zero, Time.deltaTime * _cameraSpeedY);


        }
        else if (Mathf.Abs(_diffCamera.y) > 0)
        {
            camera = Vector3.Lerp(camera, Vector3.zero, Time.deltaTime * _cameraSpeedY);
        }
        return 0;
        return camera.y;
    }
}
