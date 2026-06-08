using System;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{ 
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
        
        if (Input.GetKey(KeyCode.D))
            Vertical++;
        else if (Input.GetKey(KeyCode.A))
            Vertical--;
        
        Horizontal = 0;
        
        if (Input.GetKey(KeyCode.W))
            Horizontal++;
        else if (Input.GetKey(KeyCode.S))
            Horizontal--;
    }
}