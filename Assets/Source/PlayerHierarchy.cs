using System;
using UnityEngine;

[Serializable]
public sealed class PlayerHierarchy : MonoBehaviour
{
    public Rigidbody2D Rigidbody;
    public Collider2D WholeBodyCollider;
    public Collider2D FeetCollider;
    public Collider2D SideLeftCollider;
    public Collider2D SideRightCollider;

    public bool IsValid()
    {
        if (Rigidbody == null)
            return false;
        if (WholeBodyCollider == null)
            return false;
        if (FeetCollider == null)
            return false;
        if (SideLeftCollider == null)
            return false;
        if (SideRightCollider == null)
            return false;
        return true;
    }
}