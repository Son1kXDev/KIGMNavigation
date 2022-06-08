using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class PlayerNickname : MonoBehaviourPun
{
    public TextMeshProUGUI nameText;

    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;
        if (!photonView.IsMine)
        {
            nameText.text = photonView.Owner.NickName;
        }
    }

    private void Update()
    {
        Vector3 rot = cam.rotation * Vector3.forward;
        Vector3 lookPos = new Vector3(transform.position.x + rot.x, -90, transform.position.z + rot.z);
        transform.LookAt(lookPos, cam.rotation * Vector3.up);
    }
}