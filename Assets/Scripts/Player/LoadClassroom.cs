using Photon.Pun;
using UnityEngine;

public class LoadClassroom : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject playerPrefab;

    Vector3 spawn = new Vector3(-9, 1, 9);
    // Start is called before the first frame update
    /*void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab != null)
            {
                PhotonNetwork.Instantiate(playerPrefab.name, spawn, Quaternion.identity);
            }
            else
            {
                Debug.Log("Player prefab is not assigned!");
            }
        }
    }*/

    void Start()
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            if (playerPrefab != null)
            {
                SpawnPlayerCharacter();
            }
            else
            {
                Debug.Log("Player prefab is not assigned!");
            }
        }
    }

    private void SpawnPlayerCharacter()
    {
        if (photonView.IsMine)
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawn, Quaternion.identity);
        }
    }

}
