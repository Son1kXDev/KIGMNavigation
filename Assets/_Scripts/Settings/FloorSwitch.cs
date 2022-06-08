using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloorSwitch : MonoBehaviour
{
    [SerializeField] private GameObject Floor;                      //����
    [SerializeField] private List<Canvas> TriggersLabel;   //�������� ���������
    [SerializeField] private Color enableColor, disableColor;       //����� ������

    private Image _curButton;                                       //����������� ������� ������

    private void Start()
    {
        SetColor();
        _curButton = GetComponent<Image>();
        _curButton.color = enableColor;
    }

    public void SetColor()
    {
        enableColor = DataCenter.themeManager.mainThemeColor;
        disableColor = new Color(enableColor.r / 2f, enableColor.g / 2f, enableColor.b / 2f, 255 / 2);
        if (!_curButton) _curButton = GetComponent<Image>();
        _curButton.color = Floor.activeSelf ? enableColor : disableColor;
    }

    //���������/���������� �����
    public void FloorStatus()
    {
        Floor.SetActive(!Floor.activeSelf);
        foreach (var obj in TriggersLabel)
        {
            obj.gameObject.SetActive(Floor.activeSelf);
        }
        SetColor();
    }
}