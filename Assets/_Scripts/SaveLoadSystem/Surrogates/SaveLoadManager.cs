using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    private Storage storage;
    private AppData appData;

    private void Start()
    {
        storage = new Storage();
    }

    public void SaveMainData()
    {
        appData.switchMode = DataCenter.parameters.isEndPossition;
        appData.detalization = DataCenter.parameters.detalizationBool;
        appData.dimention = DataCenter.parameters.currentDimention;
        storage.Save(appData);
        LogSystem.Debug("Main data saved", "green");
    }

    public void SavePlayerData()
    {
        var player = DataCenter.Player;
        if (player == null)
        {
            LogSystem.DebugError("Player object is not found");
        }
        else
        {
            appData.possition = player.gameObject.transform.position;
            appData.endPossition = player.agent.destination;
        }
        storage.Save(appData);
        LogSystem.Debug("Player data saved", "green");
    }

    public void LoadMainData()
    {
        appData = (AppData)storage.Load(new AppData());
        DataCenter.parameters.detalizationBool = appData.detalization;
        DataCenter.parameters.currentDimention = appData.dimention;

        if (appData.dimention == 0) appData.dimention = 3;

        DataCenter.parameters.SetSwipeControl();
        DataCenter.UIHelper.UpdateUI();

        LogSystem.Debug("Main data loaded", "green");

        if (!DataCenter.parameters.isOnline) StartCoroutine(LoadPlayerData());

        DataCenter.LoadingPanel.SetActive(false);
    }

    private IEnumerator LoadPlayerData()
    {
        yield return new WaitForSeconds(0.01f);
        var curPlayer = DataCenter.Player;
        while (curPlayer == null)
        {
            curPlayer = DataCenter.Player;
            yield return null;
        }
        curPlayer.agent.Warp(appData.possition);
        DataCenter.parameters.isEndPossition = appData.switchMode;
        curPlayer.agent.SetDestination(appData.endPossition);

        DataCenter.UIHelper.UpdateUI();

        LogSystem.Debug("Player data loaded", "green");
    }
}