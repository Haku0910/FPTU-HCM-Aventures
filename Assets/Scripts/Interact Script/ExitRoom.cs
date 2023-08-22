using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ExitRoom : MonoBehaviourPunCallbacks, Interactable
{
    [SerializeField] private string prompt;
    [SerializeField] private GameObject loading;
    Vector3 targetPosition;

    bool check = false;
    Vector3 spawnLocation;
    int vitri;
    private List<LocationData> locationDatas;
    public string InteractionPromp => prompt;


    private void Start()
    {
        StartCoroutine(GetFullLocationName(OnLocationDataReceived));
    }

    public bool Interact(Interactor interactor)
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Signal", out object signalObj);
        string signal = signalObj as string;
        Debug.Log("Room: " + signal);
        if (signal != null)
        {
            Debug.Log("checK: " + locationDatas.Count);
            for (int i = 0; i < locationDatas.Count; i++)
            {
                if (signal.Equals(locationDatas[i].locationName))
                {
                    vitri = i;
                    check = true;
                }
            }
            if (check)
            {
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
                Debug.Log("khong tim ra");
            }
        }
        else
        {
            Debug.Log("Signal Null");
        }
        GameObject player = interactor.gameObject;
        teleport(spawnLocation, player);

        /*        PhotonNetwork.AutomaticallySyncScene = false;
                PhotonNetwork.LoadLevel("Playground1");*/
        return true;
    }

    public void SetData(List<LocationData> data)
    {
        locationDatas = data;
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

    void OnLocationDataReceived(List<LocationData> locationDataList)
    {
        SetData(locationDataList);
    }
}
