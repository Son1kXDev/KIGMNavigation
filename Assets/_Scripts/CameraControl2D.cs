using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl2D : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [SerializeField] private float zoomMin = 1;
    [SerializeField] private float zoomMax = 8;

    private Vector3 touch;

    private void Update()
    {
        GetTouch();
        CameraMove();
    }

    private void GetTouch()
    {
        if (Input.GetMouseButtonDown(0))
        {
            touch = camera.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void CameraMove()
    {
        if (DataCenter.parameters.cameraPause == true) return;
        if (Input.touchCount == 2)
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            Vector2 touch0LastPos = touch0.position - touch0.deltaPosition;
            Vector2 touch1LastPos = touch1.position - touch1.deltaPosition;

            float distToush = (touch0LastPos - touch1LastPos).magnitude;
            float curDistTouch = (touch0.position - touch1.position).magnitude;

            float difference = curDistTouch - distToush;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = touch - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            camera.transform.position += direction;
        }
    }

    private void Zoom(float increment)
    {
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - increment, zoomMin, zoomMax);
    }
}