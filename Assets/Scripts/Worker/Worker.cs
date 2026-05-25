using System;
using UnityEngine;
using WorkerStates;

public class Worker : MonoBehaviour
{
    [SerializeField] private Mover _mover;
    [SerializeField] private Transform _handPlace;

    private WorkerStateMachine _stateMachine;
    private ResourceItem _resource;

    public event Action<ResourceItem, Worker> ResourcePutted;
    public event Action<Worker> WorkCompleted;
    
    public bool HasResource => _resource != null;
    public Transform HandPlace => _handPlace;
    public bool IsBusy { get; private set; }
    public ResourceItem TargetResource { get;  private set;}
    public ResourceItem Resource => _resource;

    public void Initialize(Transform storageUnloadZone,Transform storagePutTarget)
    {
        _stateMachine = new WorkerStateMachine(_mover, storageUnloadZone, storagePutTarget, this);
    }

    public void SendToResource(ResourceItem resource)
    {
        TargetResource = resource;
        IsBusy = true;
        resource.SetReserved();
        _stateMachine.SetState(typeof(MoveState));
    }
    
    public void KeepResource()
    {
       _resource = TargetResource;
       TargetResource = null;

       if (_resource == null)
           return;
       
       _resource.SetParent(HandPlace);
    }

    public void PutResource()
    {
        ResourcePutted?.Invoke(_resource,  this);
        _resource = null;
        IsBusy = false;
        _stateMachine.SetState(typeof(IdleState));
        WorkCompleted?.Invoke(this);
    }
}