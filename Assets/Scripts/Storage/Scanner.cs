using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Transform _scannerItem;
    [SerializeField] private TriggerDetector _detector;
    [SerializeField] private float _delay;
    [SerializeField] private Vector3 _endScale;
    [SerializeField] private float _scanTime = 1.5f;
    
    private readonly List<ResourceItem> _scannedResources = new List<ResourceItem>();
    private WaitForSeconds _durationTime;
    private Vector3 _scannerPosition;
    private float _scannerRadius;
    private WaitForSeconds _delayTime;
    private Vector3 _startScale;

    public event Action<List<ResourceItem>> Scanned;

    void Start()
    {
        _delayTime = new WaitForSeconds(_delay);
        _durationTime = new WaitForSeconds(_scanTime);
        StartCoroutine(ScanCoroutine());
        _startScale = _scannerItem.localScale;

        _detector.TriggerEntered += TriggerEntered;
    }

    private void TriggerEntered(Collider other)
    {
        if (other.TryGetComponent(out ResourceItem resourceItem))
        {
            if (resourceItem.IsReserved || resourceItem.CanHarvest == false || _scannedResources.Contains(resourceItem))
                return;
            
            _scannedResources.Add(resourceItem);
        }
    }

    private IEnumerator ScanCoroutine()
    {
        while (true)
        {
            yield return _delayTime;
            Scan();
            yield return _durationTime;
        }
    }

    private void Scan()
    {
        DOTween.KillAll(_scannerItem);
        _scannedResources.Clear();
        _scannerItem.gameObject.SetActive(true);
        _scannerItem.DOScale(_endScale, _scanTime).OnComplete(CompleteScanning);
    }

    private void CompleteScanning()
    {
        _scannerItem.gameObject.SetActive(false);
        _scannerItem.localScale = _startScale;
        
        Scanned?.Invoke(_scannedResources);
    }
}