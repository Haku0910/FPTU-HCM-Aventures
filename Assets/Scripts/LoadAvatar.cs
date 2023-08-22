using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoadAvatar : MonoBehaviourPunCallbacks
{

    [SerializeField]
    public GameObject playerPrefab;
    public List<LocationData> locationDatas = new List<LocationData>();

    Vector3 defaultSpawn = new Vector3(-50, 2, 32);
    Vector3 spawnLocation = new Vector3(65, 0, 25);
    private Transform playerTransform;

    private string signal;
    private int vitri;

    private bool isAwakeCompleted = false;
    private const string HasAvatarPropertyKey = "HasAvatar";
    public static LoadAvatar instance;
    public ExitRoom exitRoom;

    bool isChangingScene = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this; // Gán thể hiện đầu tiên của PlayerManager cho biến instance
            DontDestroyOnLoad(gameObject); // Đảm bảo GameObject PlayerManager không bị huỷ khi chuyển scene
        }
        else
        {
            Destroy(gameObject); // Nếu có thể hiện khác của PlayerManager đã tồn tại, huỷ GameObject này đi
        }
        StartCoroutine(GetFullLocationName(OnLocationDataReceived));

    }

    private void Start()
    {
        StartCoroutine(WaitForAwakeCompletion());
    }

    private IEnumerator WaitForAwakeCompletion()
    {
        while (!isAwakeCompleted)
        {
            yield return null; // Đợi một frame
        }
        if (isAwakeCompleted)
        {
            if (PhotonNetwork.IsConnectedAndReady)
            {
                if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Signal", out object signalObj))
                {
                    signal = signalObj as string;
                    Debug.Log("Signal PlayGround: " + signal);
                    if (signal != null)
                    {
                        for (int i = 0; i < locationDatas.Count; i++)
                        {
                            if (locationDatas[i].locationName.Equals(signal))
                            {
                                vitri = i;
                            }
                        }
                        double x = locationDatas[vitri].x;
                        spawnLocation.x = float.Parse(x.ToString());
                        double y = locationDatas[vitri].y;
                        spawnLocation.y = float.Parse(y.ToString());
                        double z = locationDatas[vitri].z;
                        spawnLocation.z = float.Parse(z.ToString());
                        Debug.Log("vi tri: " + x + ", " + y + ", " + z);
                        Debug.Log("toa do: " + spawnLocation);
                    }
                    else
                    {
                        Debug.Log("Signal Null");
                    }
                }
                if (playerPrefab != null)
                {

                    if (signal == null)
                    {
                        playerTransform = PhotonNetwork.Instantiate(playerPrefab.name, defaultSpawn, Quaternion.identity).transform;

                    }
                    else
                    {
                        playerTransform = PhotonNetwork.Instantiate(playerPrefab.name, spawnLocation, Quaternion.identity).transform;
                    }


                }
                else
                {
                    Debug.Log("Player prefab is not assigned!");
                }

            }
        }
        else
        {
            Debug.Log("Not Done Yet");
        }
    }

    public IEnumerator GetFullLocationName(Action<List<LocationData>> callback)
    {
        string url = $"http://anhkiet-001-site1.htempurl.com/api/Locations";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Parse JSON response to extract "data" array
                LocationListDataWrapper wrapper = JsonUtility.FromJson<LocationListDataWrapper>(response);
                List<LocationData> locationNames = new List<LocationData>();

                foreach (LocationData locationData in wrapper.data)
                {
                    locationNames.Add(locationData);
                }

                // Call the callback function with the location names list
                callback?.Invoke(locationNames);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "RoomType1") // Adjust the scene name as needed
        {
            OnLocalPlayerSceneChanged(scene, mode);
        }
    }
    public void OnLocalPlayerSceneChanged(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnLocalPlayerSceneChanged is called");

        if (scene.name == "RoomType1") // Adjust the scene name as needed
        {

            // at the correct position and re-enable network synchronization.
        }
    }
    void OnLocationDataReceived(List<LocationData> locationDataList)
    {
        // Xử lý danh sách LocationData ở đây
        foreach (LocationData locationData in locationDataList)
        {
            locationDatas.Add(locationData);
        }
        exitRoom.SetData(locationDatas);
        isAwakeCompleted = true;
    }
    /*public void SwitchToScene(string sceneName)
    {
        int id = 0;
        foreach (GameObject playerAvatar in GameObject.FindGameObjectsWithTag("Player"))
        {
            PhotonView pv = playerAvatar.GetComponent<PhotonView>();
            if (pv != null && pv.IsMine)
            {
                id = pv.ViewID;
            }
        }
        Debug.Log("ID: " + id);
        PhotonNetwork.AutomaticallySyncScene = false;
        if (PhotonNetwork.IsConnectedAndReady && photonView.IsMine)
        {
            // Chuyển cảnh khi nhận được yêu cầu từ master client
            PhotonNetwork.LoadLevel(sceneName);
            photonView.RPC("DestroyMyAvatar", RpcTarget.AllBuffered, id);
        }
        // Hủy GameObject của local player trong Persistent Scene trước khi chuyển scen
    }

    [PunRPC]
    private void DestroyMyAvatar(int viewId)
    {
        PhotonView pv = PhotonView.Find(viewId);
        Debug.Log("ID photon: " + pv);
        if (pv)
        {
            // Nếu GameObject thuộc về bản thân, hủy nó
            PhotonNetwork.Destroy(pv.gameObject);
        }
    }*/


}

