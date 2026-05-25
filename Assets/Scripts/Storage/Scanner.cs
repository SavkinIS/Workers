using System;
using System.Collections;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform _scannerItem;
    [SerializeField] private TriggerDetector _detector;
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

    void Start()
    {
        _delayTime = new WaitForSeconds(_delay);
        _durationTime = new WaitForSeconds(_scanTime);
        StartCoroutine(ScanCoroutine());
        _startScale = _scannerItem.localScale;
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
        _scannerItem.gameObject.SetActive(true);

        while (Vector3.Distance(_scannerItem.localScale, _endScale) > 1f)
        {
            _scannerItem.localScale = Vector3.Lerp(
                _scannerItem.localScale,
                _endScale,
                _speed * Time.deltaTime
            );

            var hits = Physics.OverlapSphere(_scannerPosition, _scannerItem.localScale.x, _layerMask);

            foreach (var hit in hits)
            {
                if (hit.TryGetComponent(out ResourceItem resourceItem))
                {
                    Scanned?.Invoke(resourceItem);
                }
            }

            yield return null;
        }

        _scannerItem.gameObject.SetActive(false);
        _scannerItem.localScale = _startScale;
    }
}