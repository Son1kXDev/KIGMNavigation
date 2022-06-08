using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dimension : MonoBehaviour
{
    [SerializeField] private Camera Camera3D, Camera2D;             //камеры

    [SerializeField] private Color enableColor, disableColor;       //цвета кнопки

    [SerializeField] private TextMeshProUGUI _curButtonText;        //текст текущей кнопки

    private void Start()
    {
        StartCoroutine(StartUp());
    }

    private IEnumerator StartUp()
    {
        yield return new WaitForSeconds(0.1f);
        LogSystem.Debug("Startup Dimention", "red");
        SetDimension();
    }

    private void SetText()
    {
        if (DataCenter.parameters.currentDimention == 2)
        {
            _curButtonText.text = $"Режим: 2D";
        }
        else if (DataCenter.parameters.currentDimention == 3)
        {
            _curButtonText.text = $"Режим: 3D";
        }
    }

    private void SetDimension()
    {
        //DataCenter.parameters.SetSwipeControl();
        if (DataCenter.parameters.currentDimention == 3)
        {
            Camera2D.gameObject.SetActive(false);
            Camera3D.gameObject.SetActive(true);
        }
        else if (DataCenter.parameters.currentDimention == 2)
        {
            Camera2D.gameObject.SetActive(true);
            Camera3D.gameObject.SetActive(false);
            LogSystem.Debug($"Mode: {DataCenter.parameters.currentDimention}D", "red");
        }
        SetText();
        LogSystem.Debug($"Mode: {DataCenter.parameters.currentDimention}D", "red");
    }

    public void SwitchDimension()
    {
        if (DataCenter.parameters.currentDimention == 2)
        {
            DataCenter.parameters.currentDimention = 3;
        }
        else if (DataCenter.parameters.currentDimention == 3)
        {
            DataCenter.parameters.currentDimention = 2;
        }
        SetDimension();
    }
}