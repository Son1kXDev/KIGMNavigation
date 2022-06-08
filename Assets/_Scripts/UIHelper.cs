using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIHelper : MonoBehaviour
{
    [Header("������")]
    public GameObject sendAnErrorPanel;

    public GameObject settingsPanel;
    [SerializeField] private GameObject SearchModePanel;
    [SerializeField] private GameObject chatPanel;

    [Header("��������� �������")]
    [SerializeField]
    private TextMeshProUGUI navigationModeText;

    [SerializeField]
    private TextMeshProUGUI applicationVersion;

    [Header("���� �����")]
    [SerializeField] private TMP_InputField inputRoom;

    [Header("��������� �������")]
    [SerializeField] private GameObject curOpenedPanel;

    [SerializeField] private List<GameObject> openedPanelHash;

    private void Start()
    {
        applicationVersion.text = ("������: Beta " + Application.version);
        if (PlayerPrefs.HasKey("Review")) isReview = PlayerPrefs.GetInt("Review") == 1;
    }

    private void Update()
    {
        if (Input.backButtonLeavesApp || Input.GetKeyDown(KeyCode.Escape))
        {
            SwipeBack();
        }
    }

    private void SwipeBack()
    {
        if (curOpenedPanel == null) ExitApplication();
        else Swipe(false);
    }

    private void ClosePanel(GameObject panel) => panel.GetComponent<PanelAnimation>().CloseDialog();

    public void OpenUrl(string URL) => Application.OpenURL(URL);

    public void LoadScene(string scene) => SceneManager.LoadScene(scene);

    public void Swipe(bool isRight)
    {
        switch (isRight)
        {
            case false:
                if (settingsPanel.activeSelf) return;

                PanelManager(settingsPanel);
                break;

            case true:
                switch (settingsPanel.activeSelf)
                {
                    case true:
                        PanelManager(curOpenedPanel);
                        break;

                    case false:
                        if (DataCenter.parameters.isOnline && chatPanel.activeSelf) PanelManager(chatPanel);
                        break;
                }
                break;
        }
    }

    // ���� ������ ��������
    public void InputRoom()
    {
        LogSystem.Debug("Room: " + inputRoom.text, "white");
        DataCenter.Player.FindNavigationPosition(inputRoom.text);
        inputRoom.text = "";
    }

    public void UpdateUI()
    {
        navigationModeText.text = DataCenter.parameters.isEndPossition ? "��������� �������� �������" : "��������� ��������� �������";
    }

    public void PanelManager(GameObject panel)
    {
        //�������� � �������
        switch (panel.activeSelf)
        {
            case false:
                //���������� ���������� ������ � ���
                if (curOpenedPanel != null) openedPanelHash.Add(curOpenedPanel);
                curOpenedPanel = panel;
                panel.SetActive(true);
                DataCenter.parameters.cameraPause = true;
                DataCenter.parameters.SwipeControl = true;
                break;

            case true:
                //�������� ���� �������� ������
                bool isEmpty = openedPanelHash.Count == 0;
                switch (isEmpty)
                {
                    case false:
                        curOpenedPanel = openedPanelHash[openedPanelHash.Count - 1];
                        openedPanelHash.Remove(openedPanelHash[openedPanelHash.Count - 1]);
                        break;

                    case true:
                        curOpenedPanel = null;
                        DataCenter.parameters.cameraPause = false;
                        DataCenter.parameters.SetSwipeControl();
                        break;
                }
                ClosePanel(panel);
                break;
        }
        DataCenter.SaveLoad.SaveMainData(); //����������
    }

    public void SwitchNavigationMode()
    {
        DataCenter.parameters.SwitchSetPossitionMode();
        UpdateUI();
    }

    public void ExitApplication()
    {
        DataCenter.SaveLoad.SaveMainData();
        if (DataCenter.parameters.isOnline) PhotonNetwork.Disconnect();

        //��������� ������ ��� ���� ������������

        if (!isReview) StartCoroutine(AppliactionReview());
        else StartCoroutine(QuittingProcess());
    }

    [Space(100)]
    public bool isReview = false;

    public GameObject ReviewPanel;

    public void SetReview()
    {
        isReview = true;
        PlayerPrefs.SetInt("Review", isReview ? 1 : 0);
    }

    private IEnumerator AppliactionReview()
    {
        ReviewPanel.SetActive(true);
        while (!isReview)
        {
            yield return null;
        }
        ReviewPanel.SetActive(false);
        StartCoroutine(QuittingProcess());
    }

    //����� ��������� �����

    private IEnumerator QuittingProcess()
    {
        yield return new WaitForSeconds(0.3f);

        LogSystem.Debug("Application quitting...", "yellow");

        Application.Quit();
    }
}