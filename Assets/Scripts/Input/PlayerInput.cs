using System;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _leftButton = KeyCode.A;
    [SerializeField] private KeyCode _rightButton = KeyCode.D;
    [SerializeField] private KeyCode _upButton = KeyCode.W;
    [SerializeField] private KeyCode _downButton = KeyCode.S;
    
    private const int LeftMouseButton = 0;
    
    public event Action<Vector2> MouseClicked;
    public int Horizontal {get; private set;}
    public int Vertical { get; private set; }
    public KeyCode LeftButton => _leftButton;
    public KeyCode RightButton => _rightButton;
    public KeyCode UpButton => _upButton;
    public KeyCode Downutton => _downButton;

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            MouseClicked?.Invoke(Input.mousePosition);
        }

        Vertical = 0;
        
        if (Input.GetKey(_rightButton))
            Vertical++;
        else if (Input.GetKey(_leftButton))
            Vertical--;
        
        Horizontal = 0;
        
        if (Input.GetKey(_upButton))
            Horizontal++;
        else if (Input.GetKey(_downButton))
            Horizontal--;
    }
}