using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using TMPro;

public class LobbyManager : MonoBehaviourPunCallbacks
{
    public TMP_InputField RoomName;
    public GameObject ErrorPanel;
    public TextMeshProUGUI errorPanelText;

    public void CreateRoom()
    {
        if (RoomName.text.Length > 3)
        {
            PlayerPrefs.SetString("RoomName", RoomName.text);
            RoomOptions roomOptions = new RoomOptions();
            roomOptions.MaxPlayers = 20;
            PhotonNetwork.CreateRoom(RoomName.text, roomOptions);
        }
        else
        {
            ErrorPanel.SetActive(true);
            errorPanelText.text = $"Ошибка:\nКороткое название комнаты";
        }
    }

    public void JoinRoom()
    {
        if (RoomName.text.Length > 3)
        {
            PlayerPrefs.SetString("RoomName", RoomName.text);
            PhotonNetwork.JoinRoom(RoomName.text);
        }
        else
        {
            ErrorPanel.SetActive(true);
            errorPanelText.text = $"Ошибка:\nКороткое название комнаты";
        }
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        if (message == "Game does not exist")
        {
            ErrorPanel.SetActive(true);
            errorPanelText.text = $"Ошибка:\nИгра не найдена";
        }
    }

    public override void OnJoinedRoom() => PhotonNetwork.LoadLevel("Online");

}