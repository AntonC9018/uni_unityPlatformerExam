using UnityEngine;

public sealed class AnimationController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerHierarchy _hierarchy;
    [SerializeField] private PlayerStateManager _stateManager;
    
    void Start()
    {
        if (_animator == null)
            this.FailInitialization();
        if (!_hierarchy.IsValid())
            this.FailInitialization();
        
        _stateManager.OnMovingStateChanged += (oldState, newState) =>
        {
            if ((newState & (PlayerStates.Jumping | PlayerStates.Falling)) != 0)
            {
                _animator.Play("Jumping");
                return;
            }
            
            if ((newState & PlayerStates.Moving) != 0)
            {
                _animator.Play("Walking");
                return;
            }
            
            {
                _animator.Play("Idle");
                return;
            }
        };
    }
}
