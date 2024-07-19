using System;
using UnityEngine;

public class Logger
{
    public static void Log(object message)
    {
    #if !UNITY_EDITOR
        if (CheatsManager.Instance != null && CheatsManager.Instance.AreCheatsEnabled)
    #endif
        {
            Debug.Log (message);
        }
    }

    public static void LogWarning(object message)
    {
    #if !UNITY_EDITOR
        if (CheatsManager.Instance != null && CheatsManager.Instance.AreCheatsEnabled)
    #endif
        {
            Debug.LogWarning (message);
        }
    }

    public static void LogError(object message)
    {
    #if !UNITY_EDITOR
        if (CheatsManager.Instance != null && CheatsManager.Instance.AreCheatsEnabled)
    #endif
        {
            Debug.LogError (message);
        }
    }

    public static void LogException(Exception message)
    {
    #if !UNITY_EDITOR
        if (CheatsManager.Instance != null && CheatsManager.Instance.AreCheatsEnabled)
    #endif
        {
            Debug.LogException (message);
        }
    }
}
