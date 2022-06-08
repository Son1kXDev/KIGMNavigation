using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class AsynsLoading : MonoBehaviour
{
    public int sceneId;
    [SerializeField] private TextMeshProUGUI loadingText;
    [SerializeField] private Slider loadingBar;

    private void Start()
    {
        StartCoroutine(AsynsLoadingOperation());
        StartCoroutine(LoadingTextAnimation());
    }

    private IEnumerator AsynsLoadingOperation()
    {
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneId);

        while (!async.isDone)
        {
            loadingBar.value = async.progress;

            LogSystem.Debug($"Loading: {async.progress}", "green");

            yield return null;
        }
    }

    private IEnumerator LoadingTextAnimation()
    {
        while (true)
        {
            loadingText.text = "Загрузка";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "Загрузка.";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "Загрузка..";
            yield return new WaitForSeconds(0.2f);
            loadingText.text = "Загрузка...";
            yield return new WaitForSeconds(0.2f);
        }
    }
}