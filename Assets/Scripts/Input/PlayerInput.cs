using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private KeyCode _leftBtn = KeyCode.A;
    [SerializeField] private KeyCode _rightBtn = KeyCode.D;
    [SerializeField] private KeyCode _upBtn = KeyCode.W;
    [SerializeField] private KeyCode _downButton = KeyCode.S;
    
    private const int LeftMouseButton = 0;
    
    public event Action<Vector2> MouseClicked;
    public int Horizontal {get; private set;}
    public int Vertical { get; private set; }

    private void Update()
    {
        if (Input.GetMouseButtonDown(LeftMouseButton))
        {
            MouseClicked?.Invoke(Input.mousePosition);
        }

        Vertical = 0;
        
        if (Input.GetKey(_rightBtn))
            Vertical++;
        else if (Input.GetKey(_leftBtn))
            Vertical--;
        
        Horizontal = 0;
        
        if (Input.GetKey(_upBtn))
            Horizontal++;
        else if (Input.GetKey(_downButton))
            Horizontal--;
    }
}