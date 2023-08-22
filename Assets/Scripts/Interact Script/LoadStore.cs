using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine;

public class LoadStore : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;

    Vector3 doorA = new Vector3(14, 1, 2);
    Vector3 doorB = new Vector3(-12, 1, 1);

    private PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    void Start()
    {
        if (!PhotonNetwork.IsConnected || !photonView.IsMine)
        {
            // Nếu không phải người chơi local gọi script này, thoát khỏi hàm Start
            return;
        }

        if (photonView.IsMine)
        {
            Hashtable customProperties = CustomPropertiesManager.GetCustomProperties(PhotonNetwork.LocalPlayer.UserId);
            if (customProperties != null)
            {
                if (customProperties.ContainsKey("Signal"))
                {
                    string signal = customProperties["Signal"] as string;
                    Debug.Log("Signal: " + signal);
                    if (signal.Equals("711 A"))
                    {
                        SpawnPlayerCharacter(doorA);
                    }
                    else if (signal.Equals("711 B"))
                    {
                        SpawnPlayerCharacter(doorB);
                    }
                }
                else
                {
                    Debug.Log("Signal not found");
                }
            }
            else
            {
                Debug.Log("CustomProperties not found for UserID: " + PhotonNetwork.LocalPlayer.ActorNumber);
            }
        }
    }

    // Hàm SpawnPlayerCharacter sao chép từ script LoadClassroom
    private void SpawnPlayerCharacter(Vector3 spawnPosition)
    {
        if (PhotonNetwork.IsConnectedAndReady)
        {
            GameObject existingPlayer = GameObject.FindWithTag("Player");
            if (existingPlayer != null)
            {
                existingPlayer.transform.position = spawnPosition;
            }
            else
            {
                if (playerPrefab != null)
                {
                    PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
                }
                else
                {
                    Debug.Log("Player prefab is not assigned!");
                }
            }
        }
    }
}
