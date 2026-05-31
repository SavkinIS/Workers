using System;
using Configs;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private WorkerConfig _config;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private Transform _target;
    //private Rotator _rotator;
    private float _arrivalThreshold;
    
    public event Action DestinationReached;

    private bool IsDestinationReached { get; set; }
    private bool HasTarget => _target != null;
    private bool CanMove => HasTarget && !IsDestinationReached;

    private void Awake()
    {
        IsDestinationReached = true;

        //_rotator = new Rotator(transform);
        _navMeshAgent.speed = _config.MoveSpeed;
    }

    public void Move()
    {
        if (CanMove)
        {
            if (!_navMeshAgent.pathPending &&
                _navMeshAgent.remainingDistance <= _arrivalThreshold)
            {
                SetTarget(null);
            }

            //_rotator.Rotate();
            //transform.position = newPos;
        }
    }

    public void SetTarget(Transform target, bool toBase = false)
    {
        _target = target;

        if (toBase)
            _arrivalThreshold = _config.ArrivalBaseThreshold;
        else
            _arrivalThreshold = _config.ArrivalThreshold;

        _navMeshAgent.stoppingDistance = _arrivalThreshold;

        IsDestinationReached = target == null;
        //_rotator.SetTarget(target);

        if (IsDestinationReached)
        {
            _navMeshAgent.speed = 0;
            _navMeshAgent.isStopped = true;
            DestinationReached?.Invoke();
        }
        else
        {
            _navMeshAgent.SetDestination(target.position);
            _navMeshAgent.isStopped = false;
            _navMeshAgent.speed =  _config.MoveSpeed;
        }
    }
}