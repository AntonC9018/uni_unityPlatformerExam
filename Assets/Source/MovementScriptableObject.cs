#nullable enable
using UnityEngine;

[CreateAssetMenu(fileName = "Movement", menuName = "ScriptableObjects/Movement", order = 1)]
public sealed class MovementScriptableObject : ScriptableObject
{
    public float HorizontalSpeed = 1f;
    public float JumpForce = 1f;
}