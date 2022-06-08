using UnityEngine;

public class LogSystem : MonoBehaviour
{
    public static void Debug(string DebugString, string DebugColor)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.Log($"<color={DebugColor}>{DebugString}</color>");
#endif
    }

    public static void DebugError(string DebugString)
    {
#if UNITY_EDITOR
        UnityEngine.Debug.LogError(DebugString);
#endif
    }
}