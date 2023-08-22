using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class LoadLocationAPI : MonoBehaviour
{
    private const string API_URL = "http://anhkiet-001-site1.htempurl.com/api/Locations";

    public event Action<List<LocationData>> LocationDataLoadedEvent;
    private List<LocationData> locationNames = new List<LocationData>();
    private List<LocationData> location = new List<LocationData>();
    private bool dataLoaded = false;

    private void Awake()
    {
        StartCoroutine(GetFullLocationName());
    }

    private IEnumerator GetFullLocationName()
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(API_URL))
        {
            string authToken = PlayerPrefs.GetString("token");
            webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                LocationListDataWrapper wrapper = JsonUtility.FromJson<LocationListDataWrapper>(response);
                locationNames = wrapper.data;
                dataLoaded = true; // Đánh dấu rằng dữ liệu đã được tải
                LocationDataLoadedEvent?.Invoke(locationNames);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }

    public List<LocationData> GetLocation()
    {
        if (dataLoaded) // Kiểm tra xem dữ liệu đã được tải xong hay chưa
        {
            Debug.Log("API GET: " + locationNames.Count);
            return locationNames;
        }
        return null;
    }
}
