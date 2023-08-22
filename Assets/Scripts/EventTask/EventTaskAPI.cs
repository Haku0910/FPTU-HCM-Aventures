using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class EventTaskAPI : MultiplayerSingleton<EventTaskAPI>
{
    public IEnumerator GetEventTaskByTaskId(string taskId, Action<string> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/EventTasks/eventtask/{taskId}";
        Debug.Log(url);
        using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
        {
            /* string authToken = PlayerPrefs.GetString("token");
             webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);*/
            yield return webRequest.SendWebRequest();

            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                string response = webRequest.downloadHandler.text;
                // Parse JSON response to extract "data" array
                EventTaskDataWrapper wrapper = JsonUtility.FromJson<EventTaskDataWrapper>(response);
                Debug.Log(wrapper.data.eventId);
                // Access the properties of majorData
                string eventID = wrapper.data.eventId;

                // Call the callback function with the majorName
                callback?.Invoke(eventID);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
}
