using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallenAfterJumpBlock : MonoBehaviour {

    [SerializeField] private int countForBreak = 3;
    [SerializeField] private ManagerController _managerController;

    private MeshRenderer _meshRenderer;
    private Rigidbody _rigidbody;
    private int _currentCount = 0;

    private void Start()
    {
        _meshRenderer = gameObject.GetComponent<MeshRenderer>();
        _rigidbody = gameObject.GetComponent<Rigidbody>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.tag.Equals("Player"))
            return;

        if (!_managerController.IsOnTheGround() && _managerController.jSpeed < 0)
        {
            _currentCount = _currentCount == countForBreak ? countForBreak : _currentCount + 1;
            Debug.Log("Was Jumped");
        }
    }

    public int GetCurrentCount()
    {
        return _currentCount;
    }

    public void SetCurrentCount(int value)
    {
        _currentCount = value;
    }

    public bool IsBroken()
    {
        return _currentCount == countForBreak;
    }

    public void SetMaterial(Material material)
    {
        _meshRenderer.material = material;
    }

    public void SetGravity(bool value)
    {
        _rigidbody.useGravity = value;
    }

    public bool GetGravity()
    {
        return _rigidbody.useGravity;
    }
}
