using System;
using UnityEngine;
using UnityEngine.Serialization;

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

[Serializable]
public struct PlayerHierarchy
{
    public Rigidbody2D Rigidbody;
    public Collider2D WholeBodyCollider;
    public Collider2D FeetCollider;
    public Collider2D SideLeftCollider;
    public Collider2D SideRightCollider;

    public readonly bool IsValid()
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


public sealed class Movement : MonoBehaviour
{
    [SerializeField] private ControlsScriptableObject _controls;
    [SerializeField] private MovementScriptableObject _movement;
    
    [SerializeField] PlayerHierarchy _hierarchy;
    private IIsColliding _feet;
    private IIsColliding _sideLeft;
    private IIsColliding _sideRight;
    private bool _isJumping;
    
    
#nullable enable
    
    void Start()
    {
        void Invalid()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            enabled = false;
            throw new InitializationFailureException();
        }

        if (!_hierarchy.IsValid())
        {
            Invalid();
            return;
        }
        _feet = _hierarchy.FeetCollider.GetComponent<IIsColliding>();
        _sideLeft = _hierarchy.SideLeftCollider.GetComponent<IIsColliding>();
        _sideRight = _hierarchy.SideRightCollider.GetComponent<IIsColliding>();
        if (_feet == null
            || _sideLeft == null 
            || _sideRight == null)
        {
            Invalid();
            return;
        }
    }

    void Update()
    {
        // This check is needed to prevent the player from sticking onto the side
        // of a platform.
        if (!_sideRight.IsColliding || !_sideRight.IsColliding)
            HandleHorizontalMovement();
        
        // This check is to prevent jumping while in the air.
        if (_feet.IsColliding)
            HandleJump();
        else
            // Could reset this in an event on the collision checker.
            _isJumping = false;
            
        void HandleHorizontalMovement()
        {
            // Aici nu folosesc conceptul Time.deltaTime, deoarce misc obiectul alterandu-i velocity.
            // Miscarea prin Time.deltaTime are sens numai daca miscam obiectul manual (ii resetam pozitia).
            // Atunci codul ar fi cam asa:
            // transform.position += new Vector3(horizontalInput, 0, 0) * Time.deltaTime * _movement.HorizontalSpeed;
            float horizontalInput = 0;
            if (!_sideLeft.IsColliding)
            {
                foreach (var key in _controls.Left)
                {
                    if (Input.GetKey(key))
                    {
                        horizontalInput -= 1;
                        break;
                    }
                }
            }
            if (!_sideRight.IsColliding)
            {
                foreach (var key in _controls.Right)
                {
                    if (Input.GetKey(key))
                    {
                        horizontalInput += 1;
                        break;
                    }
                }
            }

            float currentVerticalVelocity = _hierarchy.Rigidbody.velocity.y;
            float horizontalVelocity = horizontalInput * _movement.HorizontalSpeed;
            Vector2 newVelocity = new Vector2(horizontalVelocity, currentVerticalVelocity);
            _hierarchy.Rigidbody.velocity = newVelocity;
        }

        void HandleJump()
        {
            bool isJumping = false;
            foreach (var key in _controls.Jump)
            {
                if (!Input.GetKey(key))
                    continue;
                
                // Already in the air.
                // This doesn't guarantee that the player is in the air for sure,
                // it just shows whether the jump button is being pressed.
                // There is no 100% ideal solution.
                // A better approach would be checking the colliders connected to the feet collider,
                // or handling the jump is some different way (building up the jump momentum independent
                // of the ground, while the button is being pressed down, but that's way to complicated
                // to implement quickly, I'd need a day at least).
                if (_isJumping)
                    return;
                    
                float jumpForceY = _movement.JumpForce;
                Vector2 jumpForce = jumpForceY * Vector2.up;
                _hierarchy.Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                _isJumping = true;
                return;
            }

            _isJumping = false;
        }
    }
}