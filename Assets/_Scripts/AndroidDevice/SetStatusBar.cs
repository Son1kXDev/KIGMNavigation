using UnityEngine;

public class SetStatusBar : MonoBehaviour
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private void Awake()
    {
        Screen.fullScreen = false;
    }

    public void ShowBar(Color color)
    {
        var clr = color;
        AndroidUtility.ShowStatusBar(clr);
    }

#endif
}