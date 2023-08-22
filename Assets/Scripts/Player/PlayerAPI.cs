using Photon.Pun;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerAPI : MultiplayerSingleton<PlayerAPI>
{
  

    private string namePlayer;
   
    private void Start()
    {
        namePlayer = PhotonNetwork.NickName;
    }

    public IEnumerator GetPlayer(string playerId, Action<string> callback)
    {
        string url = $"https://localhost:44367/api/Players/{playerId}";
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {

            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Parse JSON response to extract "data" array
                PlayerDataWrapper wrapper = JsonUtility.FromJson<PlayerDataWrapper>(response);
                Debug.Log(wrapper.data.id);
                // Call the callback function with the majorName
                callback?.Invoke(wrapper.data.id);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
    public IEnumerator CheckPlayerByEmail(Action<string> callback)
    {
        Debug.Log(namePlayer);
        string url = $"https://localhost:44367/api/Players/player/{PhotonNetwork.NickName}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // G?i yêu c?u và ch? ph?n h?i t? API
            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i t? API
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Phân tích ph?n h?i t? API ?? xác ??nh ng??i dùng
                string response = request.downloadHandler.text;
                // Parse JSON response to extract "data" array
                PlayerDataWrapper playerDataWrapper = JsonUtility.FromJson<PlayerDataWrapper>(response);
                if (playerDataWrapper != null && playerDataWrapper.data.id != null)
                {
                    callback?.Invoke(playerDataWrapper.data.id);

                }
            }
            else
            {
                Debug.LogError("API call failed. Error: " + request.error);
            }
        }
    }

}