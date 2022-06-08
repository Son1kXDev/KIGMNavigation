using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Photon.Pun;

public class CharacterSelect : MonoBehaviour
{
    private int curCharacter = 0;

    [SerializeField] private TMP_InputField playerName;
    [SerializeField] private GameObject errorPanel;
    [SerializeField] private GameObject[] characters;

    private void Start()
    {
        switch (PlayerPrefs.HasKey("SelectedCharacter"))
        {
            case true:
                characters[curCharacter].SetActive(false);
                curCharacter = PlayerPrefs.GetInt("SelectedCharacter");
                characters[curCharacter].SetActive(true);
                break;

            case false:
                characters[curCharacter].SetActive(false);
                curCharacter = 0;
                characters[curCharacter].SetActive(true);
                break;
        }
        if (PlayerPrefs.HasKey("PlayerName"))
        {
            string PlayerName = PlayerPrefs.GetString("PlayerName");
            playerName.text = PlayerName;
            SetNickname();
        }
    }

    public void SetNickname()
    {
        string PlayerName = playerName.text;
        PhotonNetwork.NickName = PlayerName;
        PlayerPrefs.SetString("PlayerName", PlayerName);
    }

    public void ButtonNext()
    {
        characters[curCharacter].SetActive(false);

        curCharacter++;
        if (curCharacter == characters.Length) curCharacter = 0;
        characters[curCharacter].SetActive(true);

        PlayerPrefs.SetInt("SelectedCharacter", curCharacter);
    }

    public void ButtonPrevious()
    {
        characters[curCharacter].SetActive(false);

        curCharacter--;
        if (curCharacter < 0) curCharacter = characters.Length - 1;
        characters[curCharacter].SetActive(true);

        PlayerPrefs.SetInt("SelectedCharacter", curCharacter);
    }

    public void ButtonSelect()
    {
        SetNickname();
        if (playerName.text.Length > 3 && playerName.text.Length < 15)
        {
            SceneManager.LoadScene("OnlineLobby");
        }
        else
        {
            errorPanel.SetActive(true);
        }
    }
}