using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    private const float FlattenValue= 0f;
    
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _moveSpeed = 20f;
    [SerializeField] private float _edgeSize = 20f;

    [SerializeField] private MinMaxFloat _xLimits = new(-100f, 100f);
    [SerializeField] private MinMaxFloat _zLimits = new(-100f, 100f);

    private Vector3 _moveDirection;
    private Vector3 _forward;
    private Vector3 _right;
    private readonly float _directionTrashHold = 0.01f;

    private void Update()
    {
        _moveDirection = Vector3.zero;

        _forward = _camera.transform.forward;
        _right = _camera.transform.right;

        _forward.y = FlattenValue;
        _right.y = FlattenValue;

        _forward.Normalize();
        _right.Normalize();

        ButtonMove();

        if (_moveDirection.sqrMagnitude > _directionTrashHold)
        {
            transform.position += _moveDirection.normalized * (_moveSpeed * Time.deltaTime);
        }

        Vector3 pos = transform.position;

        pos.x = _xLimits.Clamp(pos.x);
        pos.z = _zLimits.Clamp(pos.z);

        transform.position = pos;
    }

    private void ButtonMove()
    {
        if (_playerInput.Horizontal > 0)
            _moveDirection += _forward;
        else if (_playerInput.Horizontal < 0)
            _moveDirection -= _forward;

        if (_playerInput.Vertical > 0)
            _moveDirection += _right;
        else if (_playerInput.Vertical < 0)
            _moveDirection -= _right;
    }
}