using UnityEngine;

public class Flag : MonoBehaviour
{
    private Vector3 _startPosition;
    
    public bool IsActive { get; private set; }

    public void Initialize()
    {
        _startPosition = transform.position;
    }
    
    public void Activate()
    {
        IsActive =  true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void ResetPosition()
    {
        transform.position = _startPosition;
        Deactivate();
    }
}