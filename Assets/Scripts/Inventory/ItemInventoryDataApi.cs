using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ItemInventoryDataApi : MultiplayerSingleton<ItemInventoryDataApi>
{
    public IEnumerator GetItemInventoryByItemName(string itemName, Action<ItemInventoryData> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/iteminventory/byname/{itemName}";

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
                Wrapper<ItemInventoryData> wrapper = JsonUtility.FromJson<Wrapper<ItemInventoryData>>(response);
                // Call the callback function with the majorName
                callback?.Invoke(wrapper.data);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }


    string baseUrl = "https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/itemInventory";
    public IEnumerator CreateItemInventory(string jsonData)
    {
        var request = new UnityWebRequest(baseUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
         string authToken = PlayerPrefs.GetString("token");
         request.SetRequestHeader("Authorization", "Bearer " + authToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("L?i: " + request.error);
        }
        else
        {
            Debug.Log("Ph?n h?i t? server: " + request.downloadHandler.text);
        }
    }

    // H�m PUT ?? c?p nh?t th�ng tin ItemInventoryData
    public IEnumerator UpdateItemInventory(ItemInventoryData itemData)
    {
        string json = JsonUtility.ToJson(itemData);
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);

        string url = $"https://anhkiet-001-site1.htempurl.com/api/ItemInventorys/{itemData.id}";

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
}
