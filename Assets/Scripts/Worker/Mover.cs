using System;
using Configs;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] private WorkerConfig _config;

    private Transform _target;
    private Rotator _rotator;
    private float _arrivalThresholdSqr;
    
    public event Action DestinationReached;

    private bool IsDestinationReached { get; set; }
    private bool HasTarget => _target != null;
    private bool CanMove => HasTarget && !IsDestinationReached;
    private float CurrentSpeed => _config.MoveSpeed;

    private void Awake()
    {
        IsDestinationReached = true;

        _rotator = new Rotator(transform);
    }

    public void Move()
    {
        if (CanMove)
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

    public void SetTarget(Transform target, bool toBase = false)
    {
        _target = target;

        if (toBase)
            _arrivalThresholdSqr = _config.ArrivalBaseThresholdSqr;
        else
            _arrivalThresholdSqr = _config.ArrivalThresholdSqr;

        IsDestinationReached = target == null;
        _rotator.SetTarget(target);

        if (IsDestinationReached)
            DestinationReached?.Invoke();
    }
}