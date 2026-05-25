using UnityEngine;

public class StorageInfo : MonoBehaviour
{
    [SerializeField] private Storage _storage;
    [SerializeField] private TMPro.TextMeshProUGUI _resourceAmount;

    private void OnEnable()
    {
        _storage.ChangedResourceAmount += UpdateResourceAmount;
    }
    
    private void OnDisable()
    {
        _storage.ChangedResourceAmount -= UpdateResourceAmount;
    }

    private void UpdateResourceAmount(int amount)
    {
        _resourceAmount.text = amount.ToString();
    }
}