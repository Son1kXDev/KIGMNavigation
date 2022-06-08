using System.Collections;
using System.Collections.Generic;
using System.Windows;
using UnityEngine;

public class SendAnError : MonoBehaviour
{
    //открытие ссылки и закрытие панели
    public void Send(int type)
    {
        string url = "";
        switch (type)
        {
            case 0:
                url = "mailto:thelordinc@inbox.ru?subject=Сообщение об ошибке&body=Нашёл ошибку. Версия приложения: " + Application.version;
                break;

            case 1:
                url = "https://t.me/son1kx";
                break;

            case 2:
                url = "https://wa.me/+79959184101?text=Нашёл ошибку. Версия приложения: " + Application.version;
                break;
        }
        if (url != "") Application.OpenURL(url);
        else LogSystem.DebugError("Send URL is null");

        DataCenter.UIHelper.PanelManager(DataCenter.UIHelper.sendAnErrorPanel);
    }
}