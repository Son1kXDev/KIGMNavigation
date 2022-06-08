using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UIHelper : MonoBehaviour
{
    [Header("Панели")]
    public GameObject sendAnErrorPanel;

    public GameObject settingsPanel;
    [SerializeField] private GameObject SearchModePanel;
    [SerializeField] private GameObject chatPanel;

    [Header("Текстовые объекты")]
    [SerializeField]
    private TextMeshProUGUI navigationModeText;

    [SerializeField]
    private TextMeshProUGUI applicationVersion;

    [Header("Поле ввода")]
    [SerializeField] private TMP_InputField inputRoom;

    [Header("Временные объекты")]
    [SerializeField] private GameObject curOpenedPanel;

    [SerializeField] private List<GameObject> openedPanelHash;

    private void Start()
    {
        applicationVersion.text = ("Версия: Beta " + Application.version);
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

    // ввод номера кабинета
    public void InputRoom()
    {
        LogSystem.Debug("Room: " + inputRoom.text, "white");
        DataCenter.Player.FindNavigationPosition(inputRoom.text);
        inputRoom.text = "";
    }

    public void UpdateUI()
    {
        navigationModeText.text = DataCenter.parameters.isEndPossition ? "Установка конечной позиции" : "Установка стартовой позиции";
    }

    public void PanelManager(GameObject panel)
    {
        //действия с панелью
        switch (panel.activeSelf)
        {
            case false:
                //сохранение предидущей панели в кэш
                if (curOpenedPanel != null) openedPanelHash.Add(curOpenedPanel);
                curOpenedPanel = panel;
                panel.SetActive(true);
                DataCenter.parameters.cameraPause = true;
                DataCenter.parameters.SwipeControl = true;
                break;

            case true:
                //проверка кеша открытой панели
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
        DataCenter.SaveLoad.SaveMainData(); //сохранение
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

        //временные строки для бета тестирования

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

    //конец временных строк

    private IEnumerator QuittingProcess()
    {
        yield return new WaitForSeconds(0.3f);

        LogSystem.Debug("Application quitting...", "yellow");

        Application.Quit();
    }
}