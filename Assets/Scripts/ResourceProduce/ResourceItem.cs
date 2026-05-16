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
    [SerializeField] Rigidbody _rigidbody;

    private ResourceItemsStates _state;
    public Transform Transform => transform;
    public bool IsReserved { get;  private set; }
    public event Action<ResourceItem> Disabled;

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

    public void SetReserved()
    {
        IsReserved = true;
    }

    public void SetParent(Transform newParent)
    {
       transform.SetParent(newParent);
    }

    public void DisablePhysics()
    {
        _rigidbody.isKinematic = true;
    }

    public void ResetPosition()
    {
       transform.localPosition = Vector3.zero;
    }

    public void Disable(Transform newParent)
    {
        SetParent(newParent);
        Disabled?.Invoke(this);
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
}
