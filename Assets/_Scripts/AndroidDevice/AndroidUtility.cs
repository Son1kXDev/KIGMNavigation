#if UNITY_ANDROID && !UNITY_EDITOR
using System;
using UnityEngine;

public static class AndroidUtility
{
    private const int MinStatusBarColorApi = 21;
    private const int SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN = 0x00000400;

    private static AndroidJavaObject activity;

    /// <summary>
    /// Shows the Android Status bar
    /// </summary>
    /// <param name="color"> Color of status bar to be used </param>
    public static void ShowStatusBar(Color color)
    {
        int androidColor = ConvertColorToAndroidColor(color);
        RunOnUiThread(() =>
            {
                using (var window = Window)
                {
                    window.Call("clearFlags", SYSTEM_UI_FLAG_LAYOUT_FULLSCREEN);
                    if (GetApi() >= MinStatusBarColorApi)
                    {
                        window.Call("setStatusBarColor", androidColor);
                    }
                    else
                    {
                        Debug.LogWarning("Changing the status bar color is not supported on Android API lower than Lollipop.");
                    }
                }
            });
    }

    private static void RunOnUiThread(Action action)
    {
        Activity.Call("runOnUiThread", new AndroidJavaRunnable(action));
    }

    private static AndroidJavaObject Activity
    {
        get
        {
            if (activity == null)
            {
                var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            }
            return activity;
        }
    }

    private static AndroidJavaObject Window
    {
        get
        {
            return Activity.Call<AndroidJavaObject>("getWindow");
        }
    }

    private static int GetApi()
    {
        using (var version = new AndroidJavaClass("android.os.Build$VERSION"))
        {
            return version.GetStatic<int>("SDK_INT");
        }
    }

    private static int ConvertColorToAndroidColor(Color color)
    {
        Color32 color32 = color;
        int alpha = color32.a;
        int red = color32.r;
        int green = color32.g;
        int blue = color32.b;
        using (var colorClass = new AndroidJavaClass("android.graphics.Color"))
        {
            int androidColor = colorClass.CallStatic<int>("argb", alpha, red, green, blue);
            return androidColor;
        }
    }
}

#endif