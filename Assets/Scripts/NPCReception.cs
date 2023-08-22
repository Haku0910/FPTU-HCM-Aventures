using Photon.Pun;
using System;
using UnityEngine;

public class NPCReception : MonoBehaviourPunCallbacks, Interactable
{
    [SerializeField] private GameObject toActivate;
    [SerializeField] private PlayerHistoryAPI PlayerHistoryAPI;

    [SerializeField] private Transform standingPoint;
    [SerializeField] private string prompt;
    private Transform avatar;

    public string InteractionPromp => prompt;

    private void RequestTaskCompletionNotification()
    {
        DateTime currentTime = DateTime.Now;
        string formattedTime = currentTime.ToString("HH:mm:ss");
        Debug.Log(formattedTime);
    }


    public void Recover()
    {
        Disable.Instance.EnableGame();

        /*mainCamera.SetActive(true);*/
        toActivate.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

    public bool Interact(Interactor interactor)
    {
        toActivate.SetActive(true);
        Disable.Instance.DisableGame();
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        return true;
    }
}
