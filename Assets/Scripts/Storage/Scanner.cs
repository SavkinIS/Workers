using System;
using System.Collections;
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

    public event Action<ResourceItem> Scanned;

    private void Start()
    {
        _delayTime = new WaitForSeconds(_delay);
        _durationTime = new WaitForSeconds(_scanTime);
        StartCoroutine(ScanCoroutine());
        _startScale = _detectionZone.localScale;
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

        while (Vector3.Distance(_detectionZone.localScale, _endScale) > 1f)
        {
            _detectionZone.localScale = Vector3.Lerp(
                _detectionZone.localScale,
                _endScale,
                _speed * Time.deltaTime
            );

            var hits = Physics.OverlapSphere(_scannerPosition, _detectionZone.localScale.x, _layerMask);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out ResourceItem resourceItem))
                {
                    Scanned?.Invoke(resourceItem);
                }
            }

            yield return null;
        }

        _detectionZone.gameObject.SetActive(false);
        _detectionZone.localScale = _startScale;
    }
}