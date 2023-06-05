using System;
using UnityEngine;

public sealed class InitializationFailureException : Exception
{
}

public static class InitializationHelper
{
    public static void FailInitialization(this MonoBehaviour component)
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        component.enabled = false;
        throw new InitializationFailureException();
    }
}

