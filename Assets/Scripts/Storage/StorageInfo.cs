using UnityEngine;

public class StorageInfo : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI _resourceAmount;

    public void UpdateResourceAmount(int amount)
    {
        _resourceAmount.text = amount.ToString();
    }
}