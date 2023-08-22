using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCNhanVienBanHang : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject toActivate;


    [SerializeField] private Transform standingPoint;
    // Start is called before the first frame update
    private async void OnTriggerEnter(Collider other)
    {
        PhotonView photonView = other.GetComponent<PhotonView>();

        if (!photonView.IsMine)
        {
            return;
        }
        toActivate.SetActive(true);

        // d?splay cursor
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
