using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class ExchangeHistoryAPI : MultiplayerSingleton<ExchangeHistoryAPI>
{   
    public IEnumerator PostRequest(string jsonData)
    {
        string exchangeHistoryURL = "https://anhkiet-001-site1.htempurl.com/api/ExchangeHistorys/exchangeHistory";
        var request = new UnityWebRequest(exchangeHistoryURL, "POST");
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
    public IEnumerator UpdateExchange(ExchangeHistoryData itemData)
    {
        string json = JsonUtility.ToJson(itemData);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        string url = $"https://anhkiet-001-site1.htempurl.com/api/ExchangeHistorys/{itemData.id}";

        UnityWebRequest request = new UnityWebRequest(url, "PUT");
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
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

    public IEnumerator GetExchangHistoryByItemName(string itemName, Action<ExchangeHistoryData> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/ExchangeHistorys/exchangehistory/byname/{itemName}";

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
                Wrapper<ExchangeHistoryData> wrapper = JsonUtility.FromJson<Wrapper<ExchangeHistoryData>>(response);
                // Call the callback function with the majorName
                callback?.Invoke(wrapper.data);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
}
