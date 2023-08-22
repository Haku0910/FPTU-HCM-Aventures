using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NamePrefab : MultiplayerSingleton<NamePrefab>
{
    // Start is called before the first frame update
    public string playerNickname;
    public double currentPoint;
    public GetItemInventoryByPlayer inventories;
    public List<ItemData> itemDatas;
    public string PlayerId;

    private int coroutineCount = 4;
    private bool allCoroutinesFinished = false;

    void Awake()
    {
        playerNickname = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("Player nickname: " + playerNickname);
        StartCoroutine(StartAllCoroutines());
    }
    private IEnumerator StartAllCoroutines()
    {
        // Bắt đầu tất cả các Coroutine
        StartCoroutine(CheckPlayerByEmail(OnPlayerIdReceived));
        StartCoroutine(CheckPlayerByPoint(OnGetPoint));
        StartCoroutine(GetlistItemInventoryByPlayerName(OnListItemInventory));
        StartCoroutine(GetlistItemByPlayerName(OnGetListItem));

        // Chờ cho tất cả các Coroutine hoàn thành
        while (!allCoroutinesFinished)
        {
            yield return null;
        }
        ContinueGame();
    }
    private void ContinueGame()
    {
        ManageButton.Instance.UpdateToCoint(currentPoint);
    }
    private void OnListItemInventory(GetItemInventoryByPlayer ItemInventorys)
    {

        inventories = ItemInventorys;
        coroutineCount--;
        CheckAllCoroutinesFinished();
    }


    public void OnGetListItem(List<ItemData> listData)
    {

        itemDatas = listData;
        coroutineCount--;
        CheckAllCoroutinesFinished();
    }
    public IEnumerator GetlistItemByPlayerName(Action<List<ItemData>> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/iteminventory/{playerNickname}";

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
                Wrapper<GetItemInventoryByPlayer> wrapper = JsonUtility.FromJson<Wrapper<GetItemInventoryByPlayer>>(response);
                // Call the callback function with the majorName
                Debug.Log("data " + wrapper.data.listItem);
                callback?.Invoke(wrapper.data.listItem);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }


    public IEnumerator GetlistItemInventoryByPlayerName(Action<GetItemInventoryByPlayer> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/iteminventory/{playerNickname}";

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
                Wrapper<GetItemInventoryByPlayer> wrapper = JsonUtility.FromJson<Wrapper<GetItemInventoryByPlayer>>(response);
                // Call the callback function with the majorName
                Debug.Log("data " + wrapper.data.listItem);
                callback?.Invoke(wrapper.data);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }

    public IEnumerator GetPlayer(string playerId, Action<string> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/{playerId}";
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
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/player/{playerNickname}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            // G?i y�u c?u v� ch? ph?n h?i t? API
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i t? API
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Ph�n t�ch ph?n h?i t? API ?? x�c ??nh ng??i d�ng
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
    public IEnumerator CheckPlayerByPoint(Action<double> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/player/{playerNickname}";
        Debug.Log("NickName" + playerNickname);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            // G?i y�u c?u v� ch? ph?n h?i t? API
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i t? API
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Ph�n t�ch ph?n h?i t? API ?? x�c ??nh ng??i d�ng
                string response = request.downloadHandler.text;
                // Parse JSON response to extract "data" array
                PlayerDataWrapper playerDataWrapper = JsonUtility.FromJson<PlayerDataWrapper>(response);
                if (playerDataWrapper != null && playerDataWrapper.data.id != null)
                {
                    callback?.Invoke(playerDataWrapper.data.totalPoint);

                }
            }
            else
            {
                Debug.Log("API call failed. Error: " + request.error);
            }
        }
    }
    public IEnumerator CheckPlayer(Action<Player> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/player/{playerNickname}";
        Debug.Log("NickName" + playerNickname);
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            // G?i y�u c?u v� ch? ph?n h?i t? API
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i t? API
            if (request.result == UnityWebRequest.Result.Success)
            {
                // Ph�n t�ch ph?n h?i t? API ?? x�c ??nh ng??i d�ng
                string response = request.downloadHandler.text;
                // Parse JSON response to extract "data" array
                PlayerDataWrapper playerDataWrapper = JsonUtility.FromJson<PlayerDataWrapper>(response);
                if (playerDataWrapper != null && playerDataWrapper.data.id != null)
                {
                    callback?.Invoke(playerDataWrapper.data);

                }
            }
            else
            {
                Debug.LogError("API call failed. Error: " + request.error);
            }
        }
    }
    public void PutDataPlayer(string url, string jsonData)
    {
        StartCoroutine(PutData(url, jsonData));
    }
    public IEnumerator UpdatePlayer(Player itemData)
    {
        string json = JsonUtility.ToJson(itemData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/{itemData.id}";

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        string authToken = PlayerPrefs.GetString("token");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("PUT Error: " + request.error);
        }
        else
        {
            Debug.Log("PUT Success!");
        }
    }
    public IEnumerator PutData(string url, string jsonData)
    {
        using (UnityWebRequest www = UnityWebRequest.Put(url, jsonData))
        {
            string authToken = PlayerPrefs.GetString("token");
            www.SetRequestHeader("Authorization", "Bearer " + authToken);
            www.SetRequestHeader("Content-Type", "application/json");

            // G?i y�u c?u PUT ??n server
            yield return www.SendWebRequest();

            // Ki?m tra l?i trong qu� tr�nh g?i y�u c?u
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"PUT Error: {www.error}");
            }
            else
            {
                // Y�u c?u th�nh c�ng, x? l� ph?n h?i t? server
                Debug.Log("PUT Request Successful!");
            }
        }
    }
    private void OnGetPoint(double point)
    {


        currentPoint = point;
        coroutineCount--;
        CheckAllCoroutinesFinished();
    }
    private void OnPlayerIdReceived(string playerId)
    {

        PlayerId = playerId;
        // X? l� playerId nh?n ???c t?i ?�y
        Debug.Log("Player Id: " + playerId);
        coroutineCount--;
        CheckAllCoroutinesFinished();
    }

    private void CheckAllCoroutinesFinished()
    {
        if (coroutineCount <= 0)
        {
            allCoroutinesFinished = true;
        }
    }
}
