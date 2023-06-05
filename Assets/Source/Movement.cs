using UnityEngine;

public sealed class Movement : MonoBehaviour
{
    [SerializeField] private ControlsScriptableObject _controls;
    [SerializeField] private MovementScriptableObject _movement;
    
    [SerializeField] private PlayerHierarchy _hierarchy;
    [SerializeField] private PlayerStateManager _stateManager;
    private IIsColliding _feet;
    private IIsColliding _sideLeft;
    private IIsColliding _sideRight;
    private bool _isJumping;
    private PlayerStates _playerState;
    
#nullable enable
    
    void Start()
    {
        if (!_hierarchy.IsValid())
        {
            this.FailInitialization();
            return;
        }
        _feet = _hierarchy.FeetCollider.GetComponent<IIsColliding>();
        _sideLeft = _hierarchy.SideLeftCollider.GetComponent<IIsColliding>();
        _sideRight = _hierarchy.SideRightCollider.GetComponent<IIsColliding>();
        if (_feet == null
            || _sideLeft == null 
            || _sideRight == null)
        {
            this.FailInitialization();
            return;
        }
    }

    void Update()
    {
        var facingMask = (PlayerStates.FacingLeft | PlayerStates.FacingRight);
        var newState = _stateManager.State & facingMask;
        
        // This check is needed to prevent the player from sticking onto the side
        // of a platform.
        if (!_sideRight.IsColliding || !_sideRight.IsColliding)
        {
            int movingDirection = HandleHorizontalMovement();
            if (movingDirection != 0)
                newState |= PlayerStates.Moving;
            else
            {
                newState &= facingMask;
                if (movingDirection == 1)
                    newState |= PlayerStates.FacingRight;
                if (movingDirection == -1)
                    newState |= PlayerStates.FacingLeft;
            }
        }
        else
        {
            var rb = _hierarchy.Rigidbody;
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        
        // This check is to prevent jumping while in the air.
        if (_feet.IsColliding)
        {
            bool isTryingToDoJump = HandleJump();   
            if (isTryingToDoJump)
                newState |= PlayerStates.Jumping;
        }
        else
        {
            // Could reset this in an event on the collision checker.
            _isJumping = false;
            newState |= PlayerStates.Falling;
        }
        
        // This should probably only work with masked input, and then the
        // actual handler of the state change should happen after all handlers
        // of state have run.
        _stateManager.State = newState;
        
        int HandleHorizontalMovement()
        {
            // Aici nu folosesc conceptul Time.deltaTime, deoarce misc obiectul alterandu-i velocity.
            // Miscarea prin Time.deltaTime are sens numai daca miscam obiectul manual (ii resetam pozitia).
            // Atunci codul ar fi cam asa:
            // transform.position += new Vector3(horizontalInput, 0, 0) * Time.deltaTime * _movement.HorizontalSpeed;
            int horizontalInput = 0;
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
            return horizontalInput;
        }

        bool HandleJump()
        {
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
                    return true;
                    
                float jumpForceY = _movement.JumpForce;
                Vector2 jumpForce = jumpForceY * Vector2.up;
                _hierarchy.Rigidbody.AddForce(jumpForce, ForceMode2D.Impulse);
                _isJumping = true;
                return true;
            }

            _isJumping = false;
            return false;
        }
    }
}