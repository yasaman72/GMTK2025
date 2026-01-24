using System.Runtime.CompilerServices;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void Log(string message, bool inEnabled = true, [CallerMemberName] string caller = "")
    {
        if (!inEnabled) return;
        Debug.Log(caller + ":" + message);
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
