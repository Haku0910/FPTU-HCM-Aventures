using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class NpcAPI : MultiplayerSingleton<NpcAPI>
{
   
    public IEnumerator GetNpcName(string npcId, Action<string> callback)
    {
        string url = $"http://anhkiet-001-site1.htempurl.com/api/Npcs/{npcId}";
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
                NpcDataWrapper wrapper = JsonUtility.FromJson<NpcDataWrapper>(response);
                Debug.Log(wrapper.data.npcName);
                // Access the properties of majorData
                string npcName = wrapper.data.npcName;
                Debug.Log("NPC Name: " + npcName);

                // Call the callback function with the majorName
                callback?.Invoke(npcName);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }


}


