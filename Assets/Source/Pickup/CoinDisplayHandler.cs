using TMPro;
using UnityEngine;

public class CoinDisplayHandler : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;
    [SerializeField] private TMP_Text _text;
    
    void Start()
    {
        if (_inventory == null)
            this.FailInitialization();
        if (_text == null)
            this.FailInitialization();
        
        _inventory.CoinCountChanged += (coinCount) =>
        {
            _text.text = coinCount.ToString();
        };
    }
}
