using System.Collections.Generic;
using UnityEngine;

public sealed class IsCollidingStateManager : MonoBehaviour, IIsColliding
{
    private List<Collider2D> _touchingGroundColliders = new();
    public bool IsColliding => _touchingGroundColliders.Count > 0;

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