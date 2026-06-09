using System.Collections;
using UnityEngine;

public class DragObject : MonoBehaviour
{
    [SerializeField] private LayerMask _targetLayer;

    private Camera _camera;
    private Transform _target;
    private Coroutine _dragCoroutine;
    private float _maxRayDistance = 1000f;

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
                _target.position = hit.point;
            }

            yield return null;
        }
    }
}