using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class PlayerManager : MonoBehaviourPunCallbacks
{
    private int viewID;

    private void Start()
    {
        if (photonView.IsMine)
        {
            viewID = photonView.ViewID;
        }
    }

    public int GetViewID()
    {
        return viewID;
    }
}
