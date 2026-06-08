using System.Collections;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;

    private Camera _camera;
    private Transform _target;
    private Coroutine _dragCoroutine;
    private float _maxRayDistance = 1000f;
    private Vector3 _oldPosition;
    private Vector3 _newPosition;

    private void OnEnable()
    {
        if (_target != null)
        {
            _dragCoroutine = StartCoroutine(DragCoroutine());
        }
    }

    private void OnDisable()
    {
        if (_dragCoroutine != null)
            StopCoroutine(_dragCoroutine);
    }

    public void Initialize()
    {
        _camera = Camera.main;
    }

    public void SetTarget(Transform target)
    {
        _target = target;

        if (_target != null)
        {
            _oldPosition = _target.position;
            _dragCoroutine = StartCoroutine(DragCoroutine());
        }
    }

    public void ResetTarget()
    {
        _target = null;
        
        if (_dragCoroutine != null)
            StopCoroutine(_dragCoroutine);
    }

    private IEnumerator DragCoroutine()
    {
        while (_target != null)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, _maxRayDistance, _targetLayer))
            {
                _newPosition = hit.point;
            }
            else
            {
                _newPosition = _oldPosition;
            }

            _target.position = _newPosition;
            _oldPosition = _newPosition;

            yield return null;
        }
    }
}