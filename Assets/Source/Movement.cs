using System;
using UnityEngine;

public sealed class InitializationFailureException : Exception
{
}

public static class Layers
{
    public static bool Contains(this LayerMask mask, int layer)
    {
        return mask == (mask | (1 << layer));
    }
    public const int GroundLayer = 6;
}

public sealed class Movement : MonoBehaviour
{
    [SerializeField] private ControlsScriptableObject _controls;
    [SerializeField] private MovementScriptableObject _movement;
    
    private Rigidbody2D _rigidbody2D;
    private IIsGrounded _isGrounded;
    
#nullable enable
    
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        if (_rigidbody2D == null)
            throw new InitializationFailureException();
        
        _isGrounded = GetComponentInChildren<IIsGrounded>() ?? throw new InitializationFailureException();
    }

    void Update()
    {
        // Stop the object from rolling
        _rigidbody2D.angularVelocity = 0;
        
        // Aici nu folosesc conceptul Time.deltaTime, deoarce misc obiectul alterandu-i velocity.
        // Miscarea prin Time.deltaTime are sens numai daca miscam obiectul manual (ii resetam pozitia).
        // Atunci codul ar fi cam asa:
        // transform.position += new Vector3(horizontalInput, 0, 0) * Time.deltaTime * _movement.HorizontalSpeed;
        float horizontalInput = 0;
        foreach (var key in _controls.Left)
        {
            if (Input.GetKey(key))
            {
                horizontalInput -= 1;
                break;
            }
        }
        foreach (var key in _controls.Right)
        {
            if (Input.GetKey(key))
            {
                horizontalInput += 1;
                break;
            }
        }

        float currentVerticalVelocity = _rigidbody2D.velocity.y;
        float horizontalVelocity = horizontalInput * _movement.HorizontalSpeed;
        Vector2 newVelocity = new Vector2(horizontalVelocity, currentVerticalVelocity);
        _rigidbody2D.velocity = newVelocity;

        HandleJump();
        void HandleJump()
        {
            if (!_isGrounded.IsGrounded)
                return;
            
            foreach (var key in _controls.Jump)
            {
                if (Input.GetKey(key))
                {
                    float jumpForceY = _movement.JumpForce;
                    Vector2 jumpForce = jumpForceY * Vector2.up;
                    _rigidbody2D.AddForce(jumpForce, ForceMode2D.Impulse);
                    break;
                }
            }
        }
    }
}