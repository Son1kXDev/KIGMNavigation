using UnityEngine;

public class DataCenter : MonoBehaviour
{
    public static PlayerControlls Player;
    public static UIHelper UIHelper;
    public static SaveLoadManager SaveLoad;
    public static ThemeManager themeManager;
    public static Parameters parameters;

    public static GameObject LoadingPanel;

    private void Awake()
    {
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControlls>();
        UIHelper = GameObject.FindGameObjectWithTag("UIHelper").GetComponent<UIHelper>();
        SaveLoad = GameObject.FindGameObjectWithTag("SaveLoadManager").GetComponent<SaveLoadManager>();
        themeManager = GameObject.FindGameObjectWithTag("ThemeManager").GetComponent<ThemeManager>();
        parameters = GameObject.FindGameObjectWithTag("Parameters").GetComponent<Parameters>();

        LoadingPanel = GameObject.FindGameObjectWithTag("LoadingPanel");
    }
}