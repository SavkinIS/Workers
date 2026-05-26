using System;
using Spawner;
using UnityEngine;

public enum ResourceItemsStates 
{
    Created,
    Completed
}

public class ResourceItem : SpawnableObject
{
    [SerializeField] private MeshRenderer _renderer;
    [SerializeField] private Material _creaedMaterial;
    [SerializeField] private Material _compitedMaterial;
    [SerializeField] private Rigidbody _rigidbody;

    private ResourceItemsStates _state;
    
    public event Action<ResourceItem> Disabled;

    public Transform Transform => transform;

    public void Initialize()
    {
        _state = ResourceItemsStates.Created;
        UpdateState();
    }

    public void SetCompleted()
    {
        _state = ResourceItemsStates.Completed;
        UpdateState();
    }

    public void SetParent(Transform newParent)
    {
        transform.SetParent(newParent);
    }

    public void DisablePhysics()
    {
        _rigidbody.isKinematic = true;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }

    public void Disable(Transform newParent)
    {
        SetParent(newParent);
        Disabled?.Invoke(this);
    }

    public void AttachTo(Transform newParent)
    {
        SetParent(newParent);
        ResetPosition();
    }

    private void UpdateState()
    {
        if (_state == ResourceItemsStates.Created)
        {
            _renderer.material = _creaedMaterial;
            _rigidbody.isKinematic = true;
        }
        else
        {
            _renderer.material = _compitedMaterial;
            _rigidbody.isKinematic = false;
        }
    }
    
    private void ResetPosition()
    {
        transform.localPosition = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _rigidbody.velocity = Vector3.zero;
    }
}
