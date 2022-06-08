using UnityEngine;
using Photon.Pun;
using Photon.Chat;
using ExitGames.Client.Photon;
using TMPro;
using System.Collections;

public class ChatManager : MonoBehaviour, IChatClientListener
{
    public GameObject user;

    private ChatClient chatClient;

    private string userName;
    private string _channelName;

    [SerializeField] private TextMeshProUGUI chatText;
    [SerializeField] private TMP_InputField textMessage;

    private void Start()
    {
        StartCoroutine(Starting());
    }

    private IEnumerator Starting()
    {
        chatClient = new ChatClient(this);
        yield return new WaitForSeconds(0.2f);
        userName = user.GetComponentInChildren<PlayerNickname>().photonView.Owner.NickName;
        _channelName = PlayerPrefs.GetString("RoomName");
        chatText.richText = true;
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(userName));
    }

    private void Update()
    {
        chatClient.Service();
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendButton();
        }
    }

    public void SendButton()
    {
        if (textMessage.text.Length > 0)
        {
            chatClient.PublishMessage(_channelName, textMessage.text);
            textMessage.text = "";
        }
        textMessage.Select();
    }

    public void DebugReturn(DebugLevel level, string message)
    {
        LogSystem.Debug($"{level}, {message}", "cyan");
    }

    public void OnChatStateChange(ChatState state)
    {
        LogSystem.Debug($"Chat status: {state}", "cyan");
    }

    public void OnConnected()
    {
        chatText.text += $"\n Вы подключились к чату.";
        chatClient.Subscribe(_channelName);
    }

    public void OnDisconnected()
    {
        chatText.text += $"\n Вы были отключены от чата.";
        chatClient.Unsubscribe(new string[] { _channelName });
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < senders.Length; i++)
        {
            chatText.text += $"\n <b><color=#00FFE3>{senders[i]}</color></b>: {messages[i]}";
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            chatText.text += $"\n Вы подключены к каналу {channels[i]}";
        }
    }

    public void OnUnsubscribed(string[] channels)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            chatText.text += $"\n Вы отключены от канала {channels[i]}";
        }
    }

    public void OnUserSubscribed(string channel, string user)
    {
        chatText.text += $"\n Пользователь {user} подключился к каналу {channel}.";
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        chatText.text += $"\n Пользователь {user} отключился от канала {channel}.";
    }
}