using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Disconnect : MonoBehaviourPunCallbacks
{
    public void DisconnectRoom()
    {
        PhotonNetwork.Disconnect();
        DataCenter.SaveLoad.SaveMainData();
        SceneManager.LoadScene("MainScene");
    }

    public void DisconnectToMenu()
    {
        DataCenter.SaveLoad.SaveMainData();
        PhotonNetwork.Disconnect();
        DataCenter.UIHelper.LoadScene("OnlineConnecting");
    }
}