using Photon.Pun;
using UnityEngine;

public class ExitHall : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt;
    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Exit Hall");
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "Signal", gameObject.name } });
        PhotonNetwork.LoadLevel("Playground1");
        return true;
    }
}
