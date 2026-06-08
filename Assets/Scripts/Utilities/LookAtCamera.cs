using System.Collections;
using UnityEngine;

[ExecuteAlways]
public class LookAtCamera : MonoBehaviour
{
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
        StartCoroutine(ChangeRotationCoroutine());
    }
    
    [ContextMenu("Update Rotation")]
    public void UpdateRotation()
    {
        if (_camera == null)
            _camera = Camera.main;

        if (_camera != null)
            transform.LookAt(_camera.transform);
    }
    
    private IEnumerator ChangeRotationCoroutine()
    {
        while (gameObject.activeSelf)
        {
            UpdateRotation();
            yield return null;
        }
    }
    
}
