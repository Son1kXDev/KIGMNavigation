using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

public class SendAnError : MonoBehaviour
{
    //�������� ������ � �������� ������
    public void Send(int type)
    {
        string url = "";
        switch (type)
        {
            case 0:
                url = "mailto:thelordinc@inbox.ru?subject=��������� �� ������&body=����� ������. ������ ����������: " + Application.version;
                break;

            case 1:
                url = "https://t.me/son1kx";
                break;

            case 2:
                url = "https://wa.me/+79959184101?text=����� ������. ������ ����������: " + Application.version;
                break;
        }
        if (url != "") Application.OpenURL(url);
        else LogSystem.DebugError("Send URL is null");

        DataCenter.UIHelper.PanelManager(DataCenter.UIHelper.sendAnErrorPanel);
    }
}