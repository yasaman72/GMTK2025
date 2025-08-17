using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void Log(string message, bool inEnabled = true)
    {
        if (!inEnabled) return;
        Debug.Log(message);
    }
    public static void LogWarning(string message, bool inEnabled = true)
    {
        if (!inEnabled) return;
        Debug.LogWarning(message);
    }
    public static void LogError(string message)
    {
        Debug.LogError(message);
    }
}
