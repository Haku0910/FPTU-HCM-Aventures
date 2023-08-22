using Photon.Pun;
using UnityEngine;

public class LoadHall : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPrefab;

    Vector3 spawn = new Vector3(12, 0, 5);
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
        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Signal", out object signalObj))
        {
            string signal = signalObj as string;

            Vector3 spawnLocation = spawn;

            PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation, Quaternion.identity);
        }
        else
        {
            PhotonNetwork.Instantiate(playerPrefab.name, spawn, Quaternion.identity);
        }
    }
}
