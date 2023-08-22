using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventLoader : MultiplayerSingleton<EventLoader>
{
    private const string apiUrl = "https://anhkiet-001-site1.htempurl.com/api/Events/events/time/";
    private DataModel dataModel; // L?u tr? d? li?u nh?n ???c t? API
    private bool isDataLoaded = false;
    private string schoolId;
    private void Awake()
    {
        schoolId = PlayerPrefs.GetString("schoolId");

        Debug.Log("School " + schoolId);
        StartCoroutine(PeriodicLoadEventData());

    }


    IEnumerator PeriodicLoadEventData()
    {
        while (true)
        {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {

                yield return new WaitForSeconds(10f); // Đợi 10 giây trước khi gọi API tiếp theo
                yield return StartCoroutine(LoadEventData());
            }
            else
            {
                Debug.LogWarning("No internet connection. Skipping API call.");
            }
        }
    }
    IEnumerator LoadEventData()
    {
        while (true) // S? d?ng v�ng l?p v� h?n ?? ki?m tra s? ki?n li�n t?c
        {
            Debug.Log("Event url" + apiUrl + schoolId);
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl + schoolId))
            {
                string authToken = PlayerPrefs.GetString("token");
                webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log(webRequest.error);
                }
                else
                {
                    string responseText = webRequest.downloadHandler.text;
                    dataModel = JsonUtility.FromJson<DataModel>(responseText);
                    GetAllTaskData();
                    GetFirstStartTime();
                    isDataLoaded = true;

                }
            }
            yield return new WaitForSeconds(10f);
        }
    }
    public List<TaskDto> GetAllTaskData()
    {

        if (dataModel != null && dataModel.data != null)
        {

            List<TaskDto> allTasks = new List<TaskDto>();

            foreach (TaskDto task in dataModel.data.taskDtos)
            {
                allTasks.Add(task);
            }
            return allTasks;
        }

        return null;
    }
    public string GetFirstStartTime()
    {
        if (dataModel != null && dataModel.data != null)
        {
            return dataModel.data.startTime;
        }

        return null;
    }
    public bool IsDataLoaded()
    {
        return isDataLoaded;
    }
}