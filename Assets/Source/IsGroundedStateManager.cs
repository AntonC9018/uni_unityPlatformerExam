using System.Collections.Generic;
using UnityEngine;

public interface IIsGrounded
{
    bool IsGrounded { get; }
}

public sealed class IsGroundedStateManager : MonoBehaviour, IIsGrounded
{
    private List<Collider2D> _touchingGroundColliders = new();
    public bool IsGrounded => _touchingGroundColliders.Count > 0;

    void Start()
    {
        _touchingGroundColliders.Clear();
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layers.GroundLayer)
            _touchingGroundColliders.Add(collision.otherCollider);
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.layer == Layers.GroundLayer)
            _touchingGroundColliders.Remove(collision.otherCollider);
    }
}