using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RateApp : MonoBehaviour
{
    [SerializeField] private float TimerTick;
    [SerializeField] private bool isRate;
    [SerializeField] private GameObject RatePanel;

    private void Start()
    {
        StartCoroutine(RateTimer(TimerTick));
    }

    private IEnumerator RateTimer(float time)
    {
        yield return new WaitForSecondsRealtime(time);
        if (RatePanel.activeSelf == false && isRate == false) RatePanel.SetActive(true);
        if (isRate == false) yield return StartCoroutine(RateTimer(TimerTick));
    }

    public void SetRateTrue() => isRate = true;
}