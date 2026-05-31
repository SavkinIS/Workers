
using UnityEngine;
using UnityEngine.Serialization;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _edgeSize = 20f;

    [FormerlySerializedAs("_xLimits1")] [SerializeField] private MinMaxFloat _xLimits = new(-100f, 100f);
    [FormerlySerializedAs("_zLimits2")] [SerializeField] private MinMaxFloat _zLimits= new(-100f, 100f);
    private Vector3 _moveDirection;
    private Vector3 _forward;
    private Vector3 _right;

    private void Update()
    {
        _moveDirection = Vector3.zero;

         _forward = _camera.transform.forward;
         _right = _camera.transform.right;

        _forward.y = 0f;
        _right.y = 0f;

        _forward.Normalize();
        _right.Normalize();

        ButtonMove();

        if (_moveDirection == Vector3.zero)
        {
           // MouseDirection();
        }
        
        if (_moveDirection.sqrMagnitude > 0.01f)//ToDo rename
        {
            transform.position += _moveDirection.normalized * (_moveSpeed * Time.deltaTime);
        }

        Vector3 pos = transform.position;

        pos.x = _xLimits.Clamp(pos.x);
        pos.z = _zLimits.Clamp(pos.z);

        transform.position = pos;
    }

    private void MouseDirection()
    {
        Vector3 mousePos = Input.mousePosition;

        if (mousePos.x <= _edgeSize)
            _moveDirection -= _right;
         if (mousePos.x >= Screen.width - _edgeSize)
            _moveDirection += _right;

        if (mousePos.y <= _edgeSize)
            _moveDirection -= _forward;
        if (mousePos.y >= Screen.height - _edgeSize)
            _moveDirection += _forward;
    }

    private void ButtonMove()
    {
        if (Input.GetKey(KeyCode.W))
            _moveDirection += _forward;
        else if (Input.GetKey(KeyCode.S))
            _moveDirection -= _forward;

        if (Input.GetKey(KeyCode.D))
            _moveDirection += _right;
        else if (Input.GetKey(KeyCode.A))
            _moveDirection -= _right;
    }
}