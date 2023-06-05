using System;
using Unity.VisualScripting;
using UnityEngine;

public sealed class InitializationFailureException : Exception
{
}

public sealed class Movement : MonoBehaviour
{
    [SerializeField] private ControlsScriptableObject _controls;
    [SerializeField] private MovementScriptableObject _movement;
    
    private Rigidbody2D _rigidbody2D;
    
#nullable enable
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null)
            throw new InitializationFailureException();
    }

    void Update()
    {
        float horizontalInput = 0;
        foreach (var key in _controls.Left)
        {
            if (Input.GetKey(key))
            {
                horizontalInput -= _movement.HorizontalSpeed;
                break;
            }
        }
        foreach (var key in _controls.Right)
        {
            if (Input.GetKey(key))
            {
                horizontalInput += _movement.HorizontalSpeed;
                break;
            }
        }

        float currentVerticalVelocity = _rigidbody2D.velocity.y;
        Vector2 newVelocity = new Vector2(horizontalInput, currentVerticalVelocity);
        _rigidbody2D.velocity = newVelocity;
    }
}