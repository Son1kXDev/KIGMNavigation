using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FloorPanel : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private Image drag;
    [SerializeField] private Transform box;
    private Vector2 openPossition, closePossition, halfPossition;
    private RectTransform boxRect;

    private bool isDrag = false;

    private void Start()
    {
        openPossition.y = box.localPosition.y;
        boxRect = box.gameObject.GetComponent<RectTransform>();
        drag = GetComponent<Image>();
        closePossition.y = openPossition.y - boxRect.rect.height;
        halfPossition.y = openPossition.y + (closePossition.y - openPossition.y) / 2;
        StartCoroutine(FadeOut());
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StopCoroutine(FadeOut());
        isDrag = true;
        var tempcolor = drag.color;
        tempcolor.a = 1f;
        drag.color = tempcolor;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
        if (eventData.delta.y > 0)
        {
            if (box.localPosition.y <= openPossition.y)
            {
                box.localPosition += new Vector3(box.localPosition.x, eventData.delta.y / canvas.scaleFactor, box.localPosition.z);
            }
        }
        if (eventData.delta.y < 0)
        {
            if (box.localPosition.y >= closePossition.y)
            {
                box.localPosition += new Vector3(box.localPosition.x, eventData.delta.y / canvas.scaleFactor, box.localPosition.z);
            }
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (box.localPosition.y > halfPossition.y)
        {
            box.localPosition = openPossition;
            isDrag = false;
        }
        else
        {
            isDrag = true;
            box.localPosition = closePossition;
        }
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1.5f);
        while (drag.color.a != 0 && !isDrag)
        {
            var tempcolor = drag.color;
            tempcolor.a -= 0.1f;
            drag.color = tempcolor;
            yield return new WaitForSeconds(0.1f);
        }
    }
}