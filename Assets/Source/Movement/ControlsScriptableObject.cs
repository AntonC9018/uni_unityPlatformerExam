using UnityEngine;

[CreateAssetMenu(fileName = "Controls", menuName = "ScriptableObjects/Controls", order = 1)]
public sealed class ControlsScriptableObject : ScriptableObject
{
    public KeyCode[] Left = { KeyCode.A, KeyCode.LeftArrow };
    public KeyCode[] Right = { KeyCode.D, KeyCode.RightArrow };
    public KeyCode[] Jump = { KeyCode.W, KeyCode.UpArrow };
    public KeyCode[] Down = { KeyCode.S, KeyCode.DownArrow };
}