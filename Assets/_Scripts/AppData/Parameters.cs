using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    public bool isOnline = false;               //онлайн режим
    public bool isEndPossition;                 //конечная/стартовая позиция
    public bool detalizationBool = true;        //детализация
    public bool SwipeControl = true;            //свайпы
    public int currentDimention = 3;            //измерение камеры
    public bool cameraPause = false;            //пауза движения камеры

    public void SwitchSetPossitionMode()
    {
        isEndPossition = !isEndPossition;
        DataCenter.UIHelper.UpdateUI();
    }

    public void SetSwipeControl()
    {
        if (DataCenter.parameters.currentDimention == 3)
        {
            DataCenter.parameters.SwipeControl = true;
        }
        else if (DataCenter.parameters.currentDimention == 2)
        {
            DataCenter.parameters.SwipeControl = false;
        }
    }
}