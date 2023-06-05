using System;
using UnityEngine;

public sealed class CoinPickupHandler : MonoBehaviour
{
    [SerializeField] private Inventory _inventory;

    void OnTriggerEnter2D(Collider2D other)
    {
        var pickup = other.GetComponent<IPickup>();
        if (pickup == null)
            throw new InvalidOperationException("Can only handle collisions with IPickup objects");
        
        switch (pickup.Kind)
        {
            case PickupItemKind.Coin:
            {
                _inventory.CoinCount += 1;
                break;
            }
            default:
                throw new NotSupportedException($"Unhandled pickup kind {pickup.Kind}");
        }
        Destroy(other.gameObject);
    }
}
