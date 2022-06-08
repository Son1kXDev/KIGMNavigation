using UnityEngine;
using UnityEngine.Events;

public class SwipeInput : MonoBehaviour
{
    private Vector2 posIn;
    private Vector2 posOut;

    public UnityEvent onSwipeUp;
    public UnityEvent onSwipeDown;
    public UnityEvent onSwipeRight;
    public UnityEvent onSwipeLeft;

    private void Update()
    {
        if (DataCenter.parameters.SwipeControl == false)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            posIn = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            SwipeSystem();
        }
    }

    private void SwipeSystem()
    {
        posOut = Input.mousePosition;
        Vector2 delta = posIn - posOut;
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
        {
            HorizontalSwipe(delta);
        }
        else
        {
            //if (!DataCenter.UIHelper.settingsPanel.activeSelf)
            // {
            VerticalSwipe(delta);
            // }
        }
    }

    private void VerticalSwipe(Vector2 delta)
    {
        if (delta.y < -Screen.height / 4) onSwipeUp.Invoke();
        if (delta.y > Screen.height / 4) onSwipeDown.Invoke();
    }

    private void HorizontalSwipe(Vector2 delta)
    {
        if (delta.x > Screen.width / 3) onSwipeRight.Invoke();
        if (delta.x < -Screen.width / 3) onSwipeLeft.Invoke();
    }
}