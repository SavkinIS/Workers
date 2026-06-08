using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    [SerializeField] private List<Renderer> _renderers = new List<Renderer>();

    public void SetColor(Color color)
    {
        foreach (var render in _renderers)
            render.material.color = color;
    }
}