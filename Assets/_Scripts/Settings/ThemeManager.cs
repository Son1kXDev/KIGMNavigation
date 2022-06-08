using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class ThemeManager : MonoBehaviour
{
    public static int themeID = 0;  //ID текущей темы

    [Header("Смена темы")]
    public UnityEvent ThemeChanged;

    [Header("Список объектов к которым применяется тема")]
    public List<Image> MainThemeImages;

    public List<Image> SecondThemeImages;

    public List<TextMeshProUGUI> ThemeTexts;

    public List<Image> ThemeIcons;

    public Camera camera2D;

    [Header("Список тем")]
    [SerializeField] private List<Color> mainColorThemes;

    [SerializeField] private List<Color> secondColorThemes;

    [SerializeField] private List<Color> textColorThemes;

    [HideInInspector] public Color mainThemeColor;
    [HideInInspector] public Color secondColor;
    [HideInInspector] public Color textColor;

    [Space(15), SerializeField] private SetStatusBar statusBar;

    private void Awake()
    {
        StartCoroutine(Setup());
    }

    private IEnumerator Setup()
    {
        yield return new WaitForSeconds(0.001f);
        SetUpColors();
    }

    private void SetUpColors()
    {
        switch (themeID)
        {
            case 0:
                ChangeColor(mainColorThemes[0], secondColorThemes[0], textColorThemes[0]);
                break;

            case 1:
                ChangeColor(mainColorThemes[1], secondColorThemes[1], textColorThemes[0]);
                break;

            case 2:
                ChangeColor(mainColorThemes[2], secondColorThemes[2], textColorThemes[0]);
                break;

            case 3:
                ChangeColor(mainColorThemes[3], secondColorThemes[3], textColorThemes[0]);
                break;

            case 4:
                ChangeColor(mainColorThemes[4], secondColorThemes[4], textColorThemes[1]);
                break;

            case 5:
                ChangeColor(mainColorThemes[5], secondColorThemes[5], textColorThemes[0]);
                break;

            case 6:
                ChangeColor(mainColorThemes[6], secondColorThemes[6], textColorThemes[0]);
                break;
        }
        foreach (var image in MainThemeImages)
        {
            var alpha = image.color.a;
            image.color = new Color(mainThemeColor.r, mainThemeColor.g, mainThemeColor.b, alpha);
        }
        foreach (var image in SecondThemeImages)
        {
            var alpha = image.color.a;
            image.color = new Color(secondColor.r, secondColor.g, secondColor.b, alpha);
        }
        foreach (var text in ThemeTexts)
        {
            var alpha = text.color.a;
            text.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
        }
        foreach (var icon in ThemeIcons)
        {
            var alpha = icon.color.a;
            icon.color = new Color(textColor.r, textColor.g, textColor.b, alpha);
        }
        if (camera2D != null)
        {
            camera2D.backgroundColor = secondColor;
        }
        SetAndroidBarColor();
        ThemeChanged.Invoke();
        GetComponent<ThemeLoader>().Save();
    }

    public void SetAndroidBarColor()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        statusBar.ShowBar(mainThemeColor);
#endif
    }

    public void ChangeColor(Color main, Color second, Color text)
    {
        mainThemeColor = main;
        secondColor = second;
        textColor = text;
    }

    public void ChangeThemeButton(int ID)
    {
        themeID = ID;
        SetUpColors();
    }
}