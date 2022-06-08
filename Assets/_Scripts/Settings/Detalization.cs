using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Detalization : MonoBehaviour
{
    [SerializeField] private List<GameObject> DetalizationObjects;  //объекты детализации
    [SerializeField] private Color enableColor, disableColor;       //цвета кнопки

    private Image _curButton;                                       //изображение текущей кнопки

    private void Start()
    {
        _curButton = GetComponent<Image>();
        SetDetalizationMode();
    }

    public void SetColor()
    {
        enableColor = DataCenter.themeManager.mainThemeColor;
        disableColor = new Color(enableColor.r / 2f, enableColor.g / 2f, enableColor.b / 2f, 255 / 2);
        if (_curButton == null) _curButton = GetComponent<Image>();
        _curButton.color = DataCenter.parameters.detalizationBool ? enableColor : disableColor;
    }

    public void SetDetalizationMode()
    {
        foreach (GameObject detal in DetalizationObjects)
        {
            detal.SetActive(DataCenter.parameters.detalizationBool);
        }
        SetColor();
    }

    public void SwitchDetalizationMode()
    {
        DataCenter.parameters.detalizationBool = !DataCenter.parameters.detalizationBool;
        SetDetalizationMode();
    }
}