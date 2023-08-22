using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnterRoom : MonoBehaviourPunCallbacks, Interactable
{

    [SerializeField] private string prompt;

    [SerializeField] private GameObject TpPlace;
    [SerializeField] private GameObject sceenActive;
    [SerializeField] private GameObject loading;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private TMP_Text textNotification;
    [SerializeField] private Button confirmButton;

    Vector3 targetPosition;

    public LoadLocationAPI loadLocationAPI;
    List<LocationData> location;

    private void Start()
    {
        loadLocationAPI.LocationDataLoadedEvent += HandleLocationDataLoaded;
        confirmButton.onClick.AddListener(closeConfirm);
    }

    private void HandleLocationDataLoaded(List<LocationData> locationDataList)
    {
        location = locationDataList;
    }

    public string InteractionPromp
    {
        get
        {
            if (prompt == null)
            {
                return gameObject.name;
            }
            else
            {
                return prompt;
            }
        }
    }

    public bool Interact(Interactor interactor)
    {
        string name = null;
        for (int i = 0; i < location.Count; i++)
        {
            if (location[i].locationName.Equals(gameObject.name))
            {
                name = location[i].locationName;
                if (location[i].status.Equals("ACTIVE"))
                {
                    Debug.Log("Contact enter room");
                    PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Signal", gameObject.name } });
                    sceenActive.gameObject.SetActive(true);
                    //targetPosition = new Vector3(15, 1, -156);
                    targetPosition = TpPlace.transform.position;

                    GameObject player = interactor.gameObject;

                    teleport(targetPosition, player);
                    return true;
                }
            }
        }
        confirmPanel.gameObject.SetActive(true);
        textNotification.text = name.ToString() + " is inactive";

        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });

        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        return false;
    }

    public void teleport(Vector3 targetPosition, GameObject player)
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
        loading.gameObject.SetActive(true);
        yield return new WaitForSeconds(2.0f);

        // Thay đổi vị trí của avatar
        player.transform.position = newPosition;

        yield return new WaitForSeconds(2.0f);
        loading.gameObject.SetActive(false);
        player.SetActive(true);
    }

    public void closeConfirm()
    {
        confirmPanel.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });

        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
    }
}
