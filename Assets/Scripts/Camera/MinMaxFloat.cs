using UnityEngine;

[System.Serializable]
public struct MinMaxFloat
{
    public MinMaxFloat(float min, float max)
    {
        Min = min;
        Max = max;
    }
    
    public float Min;
    public float Max;

    public float Clamp(float value)
    {
        return Mathf.Clamp(value, Min, Max);
    }
}