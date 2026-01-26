using System.Runtime.CompilerServices;
using UnityEngine;

public class Logger : MonoBehaviour
{
    public static void Log(string message, bool inEnabled = true, [CallerMemberName] string caller = "")
    {
#if DEBUG
        if (!inEnabled) return;
        Debug.Log(caller + ":" + message);
#endif
    }
    public static void LogWarning(string message, bool inEnabled = true)
    {
#if DEBUG
        if (!inEnabled) return;
        Debug.LogWarning(message);
#endif
    }
    public static void LogError(string message)
    {
#if DEBUG
        Debug.LogError(message);
#endif
    }
}
