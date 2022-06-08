using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectionWaiter : MonoBehaviour
{
    [SerializeField] private GameObject connectionErrorPanel;

    private void Start()
    {
        StartCoroutine("Waiter");
    }

    private IEnumerable Waiter()
    {
        yield return new WaitForSecondsRealtime(10);
        connectionErrorPanel.SetActive(true);
        yield break;
    }
}