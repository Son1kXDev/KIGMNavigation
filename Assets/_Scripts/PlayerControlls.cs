using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;
using Photon.Pun;

public class PlayerControlls : MonoBehaviour
{
    [HideInInspector]
    public Camera mainCamera;                           //основная камера

    [HideInInspector]
    public NavMeshAgent agent;                          //navmesh агент

    private LineRenderer lineRenderer;                  //линия для отрисовки маршрута

    private PhotonView view;

    public PlayerControlls playerControlls;

    private void Start()
    {
        mainCamera = Camera.main;
        agent = GetComponent<NavMeshAgent>();
        lineRenderer = GetComponent<LineRenderer>();
        if (DataCenter.parameters.isOnline) SetOnlineProperties();
    }

    private void SetOnlineProperties()
    {
        view = GetComponent<PhotonView>();
        switch (view.IsMine)
        {
            case true:
                GameObject.FindGameObjectWithTag("ChatManager").GetComponent<ChatManager>().user = gameObject;
                LogSystem.Debug("Chat user is loaded", "white");
                break;

            case false:
                playerControlls.enabled = false;
                LogSystem.Debug("Other player is loaded", "white");
                break;
        }
    }

    private void Update() => CheckPath();

    private void CheckPath()
    {
        switch (agent.hasPath)
        {
            case true:
                if (lineRenderer.enabled == false) { lineRenderer.enabled = true; }
                DrawPath();
                break;

            case false:
                ResetPath();
                break;
        }
    }

    private void ResetPath()
    {
        if (lineRenderer.enabled == true) lineRenderer.enabled = false;
    }

    private void DrawPath()
    {
        lineRenderer.positionCount = agent.path.corners.Length;
        Vector3[] possitions = agent.path.corners;
        for (int i = 0; i < possitions.Length; i++)
        {
            possitions[i].y += 0.2f;
        }
        lineRenderer.SetPositions(possitions);
    }

    private void ClearPath()
    {
        agent.ResetPath();
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPositions(agent.path.corners);
    }

    public void FindNavigationPosition(string RoomNumber)
    {
        GameObject room = GameObject.Find(RoomNumber);
        if (room != null)
        {
            switch (DataCenter.parameters.isEndPossition)
            {
                case true:
                    agent.SetDestination(room.transform.position);
                    break;

                case false:
                    ClearPath();
                    Vector3 possition = new Vector3(room.transform.position.x, room.transform.position.y, room.transform.position.z);
                    agent.Warp(possition);
                    break;
            }
            if (!DataCenter.parameters.isOnline) DataCenter.SaveLoad.SavePlayerData();
        }
        else LogSystem.Debug($"Room {RoomNumber}  is not exist", "red");
    }
}