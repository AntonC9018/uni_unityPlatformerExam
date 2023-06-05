using UnityEngine;

public interface IPickup
{
    PickupItemKind Kind { get; }
}

public enum PickupItemKind
{
    Coin,
}

public sealed class Pickup : MonoBehaviour, IPickup
{
    private PickupItemKind _itemKind;
    public PickupItemKind Kind => _itemKind;
}
