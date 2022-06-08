using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class SearchableDropDown : MonoBehaviour
{
    [SerializeField] private GameObject buttonsPrefab = null;
    [SerializeField] private int maxScrollRectSize = 180;
    [SerializeField] private List<string> avlOptions = new List<string>();
    [SerializeField] private ScrollRect scrollRect = null;

    private Button ddButton = null;
    private TMP_InputField inputField = null;
    private Transform content = null;
    private RectTransform scrollRectTrans;
    private bool isContentHidden = true;
    private List<Button> initializedButtons = new List<Button>();

    public delegate void OnValueChangedDel(string val);

    public OnValueChangedDel OnValueChangedEvt;

    private void Start()
    {
        SetButtonPrefabScale();
        Init();
    }

    public void SetButtonPrefabScale()
    {
        buttonsPrefab.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width, 150);
        maxScrollRectSize = Screen.height / 2 - Screen.height / 10;
    }

    public void SetColor()
    {
        LogSystem.Debug("Set Color Function", "red");

        RemoveItemInScrolRect();
    }

    private void RemoveItemInScrolRect()
    {
        foreach (var option in initializedButtons)
        {
            Destroy(option.gameObject);
        }
        initializedButtons.Clear();
        Init();
        DataCenter.SaveLoad.LoadMainData();

        LogSystem.Debug("Set Color Function is Done", "green");
    }

    /// <summary>
    /// Initilize all the Fields
    /// </summary>
    private void Init()
    {
        if (ddButton == null) ddButton = GetComponentInChildren<Button>();
        //if (scrollRect == null) scrollRect = GetComponentInChildren<ScrollRect>();
        if (inputField == null) inputField = GetComponentInChildren<TMP_InputField>();
        if (scrollRectTrans == null) scrollRectTrans = scrollRect.GetComponent<RectTransform>();
        if (content == null) content = scrollRect.content;

        inputField.onValueChanged.AddListener(OnInputvalueChange);
        inputField.onEndEdit.AddListener(OnEndEditing);

        AddItemToScrollRect(avlOptions);
    }

    /// <summary>
    /// public method to get the selected value
    /// </summary>
    /// <returns> </returns>
    public string GetValue()
    {
        return inputField.text;
    }

    //call this to Add items to Drop down
    public void AddItemToScrollRect(List<string> options)
    {
        foreach (var option in options)
        {
            var buttObj = Instantiate(buttonsPrefab, content);
            buttObj.GetComponentInChildren<TMP_Text>().text = option;
            buttObj.name = option;
            buttObj.SetActive(true);
            var butt = buttObj.GetComponent<Button>();
            butt.onClick.AddListener(delegate { OnItemSelected(buttObj); });
            initializedButtons.Add(butt);
        }
        ResizeScrollRect();
        scrollRect.gameObject.SetActive(false);
    }

    /// <summary>
    /// listner To Input Field End Editing
    /// </summary>
    /// <param name="arg"> </param>
    private void OnEndEditing(string arg)
    {
        if (string.IsNullOrEmpty(arg))
        {
            Debug.Log("no value entered ");
            return;
        }
        StartCoroutine(CheckIfValidInput(arg));
    }

    /// <summary>
    /// Need to wait as end inputField and On option button Contradicted and message was poped after
    /// selection of button
    /// </summary>
    /// <param name="arg"> </param>
    /// <returns> </returns>
    private IEnumerator CheckIfValidInput(string arg)
    {
        yield return new WaitForSeconds(1);

        OnValueChangedEvt?.Invoke(inputField.text);
    }

    /// <summary>
    /// Called ever time on Drop down value is changed to resize it
    /// </summary>
    private void ResizeScrollRect()
    {
        var count = content.transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
        var length = buttonsPrefab.GetComponent<RectTransform>().sizeDelta.y * count;

        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)content.transform);
        //var length = content.GetComponent<RectTransform>().sizeDelta.y;

        scrollRectTrans.sizeDelta = length > maxScrollRectSize ? new Vector2(scrollRectTrans.sizeDelta.x,
            maxScrollRectSize) : new Vector2(scrollRectTrans.sizeDelta.x, length);
    }

    /// <summary>
    /// listner to the InputField
    /// </summary>
    /// <param name="arg0"> </param>
    private void OnInputvalueChange(string arg0)
    {
        if (!avlOptions.Contains(arg0))
        {
            FilterDropdown(arg0);
        }
    }

    /// <summary>
    /// remove the elements from the dropdown based on Filters
    /// </summary>
    /// <param name="input"> </param>
    public void FilterDropdown(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            foreach (var button in initializedButtons)
                button.gameObject.SetActive(true);
            ResizeScrollRect();
            scrollRect.gameObject.SetActive(false);
            return;
        }

        var count = 0;
        foreach (var button in initializedButtons)
        {
            if (!button.name.ToLower().Contains(input.ToLower()))
            {
                button.gameObject.SetActive(false);
            }
            else
            {
                button.gameObject.SetActive(true);
                count++;
            }
        }

        SetScrollActive(count > 0);
        ResizeScrollRect();
    }

    /// <summary>
    /// Listner to option Buttons
    /// </summary>
    /// <param name="obj"> </param>
    private void OnItemSelected(GameObject obj)
    {
        inputField.text = obj.name;
        foreach (var button in initializedButtons)
            button.gameObject.SetActive(true);
        isContentHidden = false;

        OnDDButtonClick();
        StopAllCoroutines();
        StartCoroutine(CheckIfValidInput(obj.name));
    }

    /// <summary>
    /// listner to arrow button on input field
    /// </summary>
    private void OnDDButtonClick()
    {
        if (GetActiveButtons() <= 0)
            return;
        ResizeScrollRect();
        SetScrollActive(isContentHidden);
    }

    /// <summary>
    /// respondisble to enable and disable scroll rect component
    /// </summary>
    /// <param name="status"> </param>
    public void SetScrollActive(bool status)
    {
        scrollRect.gameObject.SetActive(status);
        isContentHidden = !status;
        ddButton.transform.localScale = status ? new Vector3(1, -1, 1) : new Vector3(1, 1, 1);
        if (status == true)
        {
            if (DataCenter.parameters.cameraPause == false) DataCenter.parameters.cameraPause = true;
        }
        else if (DataCenter.parameters.cameraPause == true) DataCenter.parameters.cameraPause = false;
    }

    /// <summary>
    /// Return numbers of active buttons in the dropdown
    /// </summary>
    /// <returns> </returns>
    private float GetActiveButtons()
    {
        var count = content.transform.Cast<Transform>().Count(child => child.gameObject.activeSelf);
        var length = buttonsPrefab.GetComponent<RectTransform>().sizeDelta.y * count;
        return length;
    }
}