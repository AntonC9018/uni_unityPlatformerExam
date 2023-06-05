using UnityEngine;

public sealed class FacingDirectionController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteToInvert;
    public PlayerStateManager _stateManager;

    void Start()
    {
        if (_spriteToInvert == null)
            this.FailInitialization();
        if (_stateManager == null)
            this.FailInitialization();
        
        _stateManager.OnMovingStateChanged += (oldState, newState) =>
        {
            var directionStates = PlayerStates.FacingLeft | PlayerStates.FacingRight;
            if ((oldState & directionStates) != (newState & directionStates))
                _spriteToInvert.flipX  = (newState & PlayerStates.FacingLeft) != 0;
        };
    }
}