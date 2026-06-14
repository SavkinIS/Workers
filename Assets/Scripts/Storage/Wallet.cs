using System;
using UnityEngine;

public class Wallet
{
    private int _amount;

    public event Action<int> СhangedResourceAmount;
    
    public int Amount
    {
        get
        {
            return _amount;
        }
        set
        {
            _amount = Mathf.Max(0, value);
            СhangedResourceAmount?.Invoke(_amount);
        }
    }

    public bool TrySpendResource(int amount)
    {
        if (amount <= 0 || amount > Amount)
            return false;

        Amount -= amount;
        
        return true;
    }

    public void CollectResource()
    {
        Amount++;
    }
}