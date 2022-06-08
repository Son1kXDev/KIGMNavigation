using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnimationType
{ swipeLeft, swipeRight, scale }

public class PanelAnimation : MonoBehaviour
{
    public AnimationType type;

    [SerializeField] private Transform box;

    [SerializeField] private CanvasGroup Back;
    [SerializeField] private bool doBackgroundAnimation = true;

    private void OnEnable()
    {
        switch (type)
        {
            case AnimationType.swipeLeft:
                box.localPosition = new Vector2(-Screen.width * 2, 0);
                box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
                break;

            case AnimationType.swipeRight:
                box.localPosition = new Vector2(+Screen.width * 2, 0);
                box.LeanMoveLocalX(0, 0.5f).setEaseOutExpo().delay = 0.1f;
                break;

            case AnimationType.scale:
                box.transform.LeanScale(Vector2.one, 0.3f);
                break;
        }
        if (doBackgroundAnimation)
        {
            Back.alpha = 0;
            Back.LeanAlpha(0.8f, 0.5f);
        }
    }

    public void CloseDialog()
    {
        switch (type)
        {
            case AnimationType.swipeLeft:
                box.LeanMoveLocalX(-Screen.width * 2, 0.5f).setEaseOutExpo().setOnComplete(Done);
                break;

            case AnimationType.swipeRight:
                box.LeanMoveLocalX(+Screen.width * 2, 0.5f).setEaseOutExpo().setOnComplete(Done);
                break;

            case AnimationType.scale:
                box.transform.LeanScale(Vector2.zero, 0.5f).setEaseOutExpo().setOnComplete(Done);
                break;
        }
        if (doBackgroundAnimation) Back.LeanAlpha(0, 0.5f);
    }

    private void Done() => gameObject.SetActive(false);
}