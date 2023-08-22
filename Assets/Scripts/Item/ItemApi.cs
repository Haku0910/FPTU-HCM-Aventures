using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemApi : MultiplayerSingleton<ItemApi>
{
    private const string apiURL = "https://anhkiet-001-site1.htempurl.com/api/Items";
    public List<ItemData> items;
    public string ImageUrl;
    // Khai b�o delegate v� event ?? th�ng b�o khi d? li?u ?� s?n s�ng
    public delegate void OnDataLoaded();
    public static event OnDataLoaded onDataLoaded;

 
    void Start()
    {
        // Kh?i t?o danh s�ch c�c m?c
        items = new List<ItemData>();

        // G?i h�m ?? th?c hi?n y�u c?u API v� l?y danh s�ch c�c m?c
        StartCoroutine(GetItemsFromAPI());
    }
    public IEnumerator GetItemsFromAPI()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiURL))
        {
            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            // G?i y�u c?u ??n API
            yield return webRequest.SendWebRequest();

            // Ki?m tra n?u c� l?i trong qu� tr�nh g?i y�u c?u
            if (webRequest.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("L?i khi g?i API: " + webRequest.error);
            }
            else
            {
                // X? l� d? li?u tr? v? t? API
                string jsonResult = webRequest.downloadHandler.text;
                items = JsonUtility.FromJson<ItemDataWrapper>(jsonResult).data;

                // G?i event ?? th�ng b�o d? li?u ?� s?n s�ng
                if (onDataLoaded != null)
                {
                    onDataLoaded();
                }

                // Hi?n th? th�ng tin c�c m?c (ho?c l�m b?t c? ?i?u g� b?n mu?n v?i d? li?u)
                foreach (ItemData item in items)
                {
                    Debug.Log("ID: " + item.id + ", Name: " + item.name + ", Price: " + item.price);
                    // Ti?p t?c hi?n th? c�c thu?c t�nh kh�c c?a item n?u c?n thi?t
                }
            }
        }
    }
    public IEnumerator CheckItemById(string id, Action<ItemData> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Items/{id}";
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
                Wrapper<ItemData> playerDataWrapper = JsonUtility.FromJson<Wrapper<ItemData>>(response);
                if (playerDataWrapper != null && playerDataWrapper.data.id != null)
                {
                    Debug.Log(playerDataWrapper.data);
                    callback?.Invoke(playerDataWrapper.data);

                }
            }
            else
            {
                Debug.LogError("API call failed. Error: " + request.error);
            }
        }
    }

}
