using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThemeLoader : MonoBehaviour
{
    private void Awake()
    {
        Load();
    }

    public void Save()
    {
        PlayerPrefs.SetInt("ThemeID", ThemeManager.themeID);
        LogSystem.Debug("Theme data saved", "green");
    }

    private void Load()
    {
        if (PlayerPrefs.HasKey("ThemeID"))
        {
            ThemeManager.themeID = PlayerPrefs.GetInt("ThemeID");

            LogSystem.Debug("Theme data loaded", "green");
        }
    }
}