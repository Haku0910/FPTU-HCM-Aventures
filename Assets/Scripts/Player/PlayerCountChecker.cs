using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCountChecker : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI playerCountTextMeshPro; // Tham chi?u ??n TextMeshProUGUI ?? hi?n th? s? l??ng ng??i ch?i
    private void Start()
    {
        // Ki?m tra s? l??ng ng??i ch?i trong phòng khi vào scene
        CheckPlayerCount();
    }

    private void CheckPlayerCount()
    {
        if (PhotonNetwork.InRoom)
        {
            int playerCount = PhotonNetwork.CurrentRoom.PlayerCount;
            playerCountTextMeshPro.text = "Player In Room: " + playerCount;
            Debug.Log("S? ng??i ch?i trong phòng: " + playerCount);
        }
    }
}
