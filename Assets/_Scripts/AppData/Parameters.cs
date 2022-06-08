using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parameters : MonoBehaviour
{
    public bool isOnline = false;               //������ �����
    public bool isEndPossition;                 //��������/��������� �������
    public bool detalizationBool = true;        //�����������
    public bool SwipeControl = true;            //������
    public int currentDimention = 3;            //��������� ������
    public bool cameraPause = false;            //����� �������� ������

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