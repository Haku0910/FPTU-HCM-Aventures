using Photon.Pun;
using UnityEngine;

public class ViewList : MonoBehaviour
{
    private void Start()
    {
        PhotonView[] photonViews = FindObjectsOfType<PhotonView>();
        foreach (PhotonView photonView in photonViews)
        {
            Debug.Log("Object name: " + photonView.gameObject.name);
            Debug.Log("PhotonView ID: " + photonView.ViewID);
        }
    }
}
