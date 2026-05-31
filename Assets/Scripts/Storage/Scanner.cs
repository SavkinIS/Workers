using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform _detectionZone;
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _endScale;
    [SerializeField] private float _scanTime = 1.5f;
    [SerializeField] private float _speed = 0.2f;
    [SerializeField] private LayerMask _layerMask;

    private WaitForSeconds _durationTime;
    private Vector3 _scannerPosition;
    private float _scannerRadius;
    private WaitForSeconds _delayTime;
    private Vector3 _startScale;
    private bool _isActive = true;
    private float _scaleTrashHold = 1f;
    private float _scaleTrashHoldSqr;

    public event Action<ResourceItem> Scanned;

    private void Start()
    {
        _delayTime = new WaitForSeconds(_delay);
        _durationTime = new WaitForSeconds(_scanTime);
        StartCoroutine(ScanCoroutine());
        _startScale = _detectionZone.localScale;
        _scaleTrashHoldSqr = _scaleTrashHold * _scaleTrashHold;
    }

    private IEnumerator ScanCoroutine()
    {
        while (_isActive)
        {
            yield return _delayTime;
            yield return Scan();
            yield return _durationTime;
        }
    }

    private IEnumerator Scan()
    {
        _detectionZone.gameObject.SetActive(true);

        while ((_detectionZone.localScale - _endScale).sqrMagnitude > _scaleTrashHoldSqr)
        {
            _detectionZone.localScale = Vector3.Lerp(
                _detectionZone.localScale,
                _endScale,
                _speed * Time.deltaTime
            );
            
            yield return null;
        }
        
        var hits = Physics.OverlapSphere(_scannerPosition, _detectionZone.localScale.x / 2, _layerMask);

        Debug.DrawRay(_scannerPosition, _detectionZone.localScale / 2, Color.red);
            
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out ResourceItem resourceItem))
            {
                Scanned?.Invoke(resourceItem);
            }
        }

     

        _detectionZone.gameObject.SetActive(false);
        _detectionZone.localScale = _startScale;
    }
}