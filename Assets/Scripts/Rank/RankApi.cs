using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
[System.Serializable]
public class ApiResponseData
{
    public string data;
}
public class RankApi : MultiplayerSingleton<RankApi>
{
    public RankUI rankUI;
    public NamePrefab namePrefab;
    private Player player;
    private string SchoolId;
    public List<RankData> rankDataList = new List<RankData>();

    private void Start()
    {
        StartCoroutine(LoadPlayerAndSchool());
    }
    private IEnumerator LoadPlayerAndSchool()
    {
        yield return StartCoroutine(namePrefab.CheckPlayer(OnGetPlayer));

        yield return StartCoroutine(LoadSchool());

        yield return StartCoroutine(GetPlayerRank(OnGetPlayerRank));

        if(rankDataList != null)
        {
            RankUI.Instance.rankDataList = rankDataList;
        }

    }
    private IEnumerator LoadSchool()
    {
        yield return StartCoroutine(GetSchool(OnGetSchoolId)); ;
    }
    private void OnGetPlayer(Player dataPlayer)
    {
        player = dataPlayer;
    }
    private void OnGetSchoolId(string schoolId)
    {
        SchoolId = schoolId;
    }
    private void OnGetPlayerRank(List<RankData> rankDatas)
    {
        rankDataList = rankDatas;
    }
    public IEnumerator GetSchool(Action<string> callback)
    {        
        Debug.Log("Player.id" + player.id);

        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/playerschool/{player.id}";
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
                ApiResponseData wrapper = JsonUtility.FromJson<ApiResponseData>(response);
                // Access the properties of majorData
                string schoolId = wrapper.data;
                Debug.Log("SchoolId: " + schoolId);

                // Call the callback function with the majorName
                callback?.Invoke(schoolId);
            }
            else
            {
                Debug.Log("API call failed. Error: " + webRequest.error);
            }
        }
    }
    public IEnumerator GetPlayerRank(Action<List<RankData>> callback)
    {
        Debug.Log("Player.eventId" + player.eventId);
        Debug.Log("schoolId" + SchoolId);
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/GetRankedPlayer/{player.eventId}/{SchoolId}";
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
                Wrapper<List<RankData>> wrapper = JsonUtility.FromJson<Wrapper<List<RankData>>>(response);
                // Access the properties of majorData

                Debug.Log("Check Null:  " + wrapper.data);
                // Call the callback function with the majorName
                callback?.Invoke(wrapper.data);
            }
            else
            {
                Debug.Log("API call failed. Error: " + webRequest.error);
            }
        }
    }
}
