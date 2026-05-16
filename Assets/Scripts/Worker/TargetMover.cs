using System;
using Configs;
using UnityEngine;

public class TargetMover : MonoBehaviour
{
   [SerializeField] private WorkerConfig  _config;

    private Transform _target;
    private Rotator _rotator;
    private float _arrivalThresholdSqr;
    private float CurrentSpeed => _config.MoveSpeed;
    
    public event Action DestinationReached;
    
    public bool IsDestinationReached { get; private set; }
    
    private bool HasTarget => _target != null;

    private bool IsCanMove => HasTarget && !IsDestinationReached;

    private void Awake()
    {
        IsDestinationReached = true;
        _arrivalThresholdSqr = _config.ArrivalThresholdSqr;
        _rotator = new Rotator(transform);
    }

    public void Move()
    {
        if (IsCanMove)
        {
            MoveImplementation();
        }
    }

    public void SetTarget(Transform target)
    {
        _target = target;
        IsDestinationReached = target == null;
        _rotator.SetTarget(target);
        
        if (IsDestinationReached)
            DestinationReached?.Invoke();
    }
    
    private void MoveImplementation()
    {
        Vector3 targetPosition = _target.position;
        targetPosition.y = transform.position.y;

        float step = CurrentSpeed * Time.deltaTime;
        Vector3 newPos = Vector3.MoveTowards(transform.position, targetPosition, step);

        var distance = (transform.position - _target.position).sqrMagnitude;

        if (distance < _arrivalThresholdSqr)
        {
            SetTarget(null);
            return;
        }
        
        _rotator.Rotate();
        transform.position = newPos;
    }
}