using Photon.Pun;
using System.Collections;
using UnityEngine;

public class StatusController : MonoBehaviourPunCallbacks
{
    public static StatusController instance;
    public GameObject avatar;
    private Vector3 targetPosition; // L?u tr? v? trí m?i c?a avatar

    void Awake()
    {

        if (instance == null)
            instance = this;
    }
    public void Teleport(Vector3 targetPosition, GameObject player)
    {
        int viewID = player.GetComponent<PlayerManager>().GetViewID();
        photonView.RPC("SyncTransformPosition", RpcTarget.All, targetPosition, viewID);
    }

    [PunRPC]
    public void SyncTransformPosition(Vector3 targetPosition, int viewID)
    {
        GameObject player = PhotonView.Find(viewID).gameObject;
        StartCoroutine(TeleportWithDelay(targetPosition, player));
    }

    private IEnumerator TeleportWithDelay(Vector3 newPosition, GameObject player)
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2.0f);

        // Thay ??i v? trí c?a avatar
        player.transform.position = newPosition;

        yield return new WaitForSeconds(2.0f);
        player.SetActive(true);
    }
}

