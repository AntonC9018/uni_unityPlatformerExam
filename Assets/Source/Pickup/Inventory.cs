using System;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public event Action<int> CoinCountChanged;

    private int _coinCount;
    public int CoinCount
    {
        get => _coinCount;
        set
        {
            if (_coinCount != value)
            {
                _coinCount = value;   
                CoinCountChanged?.Invoke(_coinCount);
            }
        }
    }
}
