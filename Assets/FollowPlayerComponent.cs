using UnityEngine;

public class FollowPlayerComponent : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;

    void Start()
    {
        if (_playerTransform == null)
            this.FailInitialization();
    }
    
    void Update()
    {
        var t = transform;
        var position = t.position;
        var playerPosition = _playerTransform.transform.position;
        position.x = playerPosition.x;
        position.y = playerPosition.y;
        t.position = position;
    }
}
