using UnityEngine;

public interface IIsColliding
{
    bool IsColliding { get; }
}

public abstract class IsCollidingBase : MonoBehaviour, IIsColliding
{
    public abstract bool IsColliding { get; }
}