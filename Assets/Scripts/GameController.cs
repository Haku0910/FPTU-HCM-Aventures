using Photon.Pun;
using UnityEngine;



public class GameController : MonoBehaviour
{
    private GameObject playerPrefab;
    private GameObject playerInstance;

    void Awake()
    {
        // T�m ??n c�c GameController tr??c ?� n?u c� v� x�a b? ch�ng
        GameController[] gameControllers = FindObjectsOfType<GameController>();
        foreach (GameController gc in gameControllers)
        {
            if (gc != this)
            {
                Destroy(gc.gameObject);
            }
        }

        // Load prefab c?a Avatar t? Resources v� g�n v�o playerPrefab
        playerPrefab = Resources.Load<GameObject>("PlayerPrefab");

        // Ki?m tra n?u playerInstance ?� t?n t?i (t?c l� ?ang t? Scene 3 chuy?n v? Scene 2)
        if (playerInstance != null)
        {
            Destroy(playerInstance); // X�a playerInstance kh�ng c?n thi?t
        }

        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab != null)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
            }
            else
            {
                Debug.Log("Player prefab is not assigned!");
            }
        }
    }
}


