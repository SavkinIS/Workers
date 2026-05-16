using UnityEngine;

public class Rotator
{
    private readonly Transform _transform;
    private Transform _target;

    public Rotator(Transform transform)
    {
        _transform = transform;
    }

    public void SetTarget(Transform target)
    {
        _target = target;
    }

    public void Rotate()
    {
        if (_target != null)
        {
            Vector3 direction = _target.position - _transform.position;
            direction.y = 0; 
            _transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}