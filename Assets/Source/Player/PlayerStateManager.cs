using System;
using UnityEngine;

[Flags]
public enum PlayerStates
{
    None = 0,
    Moving = 1 << 0,
    Jumping = 1 << 1,
    Falling = 1 << 2,
    Dead = 1 << 3,
    
    FacingMask = FacingLeft | FacingRight,
    FacingLeft = 1 << 4,
    FacingRight = 1 << 5,
}

public delegate void MovingStateChanged(PlayerStates oldState, PlayerStates newState);

public interface IPlayerState
{
    public event MovingStateChanged OnMovingStateChanged;
    public PlayerStates State { get; }
}

public sealed class PlayerStateManager : MonoBehaviour, IPlayerState
{
    [SerializeField] private PlayerStates _defaultState;
    private PlayerStates _state;
    
    public event MovingStateChanged OnMovingStateChanged;

    public PlayerStates State
    {
        get => _state;
        set
        {
            if (value != _state)
            {
                OnMovingStateChanged?.Invoke(_state, value);
                _state = value;
            }
        }
    }
}