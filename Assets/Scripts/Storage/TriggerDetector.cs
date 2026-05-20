using System;
using UnityEngine;

public class TriggerDetector : MonoBehaviour 
{
    public Action<Collider> TriggerEntered;
    
    private void OnTriggerEnter(Collider other)
    {
        TriggerEntered?.Invoke(other);
    }
}