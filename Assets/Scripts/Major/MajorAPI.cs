using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class MajorAPI : MultiplayerSingleton<MajorAPI>
{
    public IEnumerator GetMajorName(string majorId, Action<string> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Major/{majorId}";
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
                MajorDataWrapper wrapper = JsonUtility.FromJson<MajorDataWrapper>(response);
                Debug.Log(wrapper.data.name);
                // Access the properties of majorData
                string majorName = wrapper.data.name;
                Debug.Log("Major Name: " + majorName);

                // Call the callback function with the majorName
                callback?.Invoke(majorName);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }


}
