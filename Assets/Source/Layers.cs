using UnityEngine;

public static class Layers
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    public const int GroundLayer = 6;
}