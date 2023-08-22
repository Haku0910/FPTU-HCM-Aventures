using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerHistoryAPI : MultiplayerSingleton<PlayerHistoryAPI>
{
    private Player player;
    private string eventID;
    private DateTime timeDateTime;
    private List<TaskDto> taskDtos;
    private PlayerHistoryData playerHistoryData;


    private void Start()
    {
        LoadEventData();
    }

    private void LoadEventData()
    {

        taskDtos = EventLoader.Instance.GetAllTaskData();
    }
    public void CheckTaskCompletionUpdate(string eventTaskId, double taskPoint, double duration)
    {
        StartCoroutine(PostPlayerHistoryData(eventTaskId, taskPoint, duration));
    }

    public IEnumerator PostPlayerHistoryData(string eventTaskId, double taskPoint, double duration)
    {
        NamePrefab namePrefab = NamePrefab.Instance;
        if (namePrefab != null)
        {
            yield return StartCoroutine(namePrefab.CheckPlayer(OnPlayerIdReceived));
        }
        Debug.Log("Player: " + player.id);
        if (player.id != null)
        {
            string apiEndpoint = "https://anhkiet-001-site1.htempurl.com/api/PlayerHistorys/playerhistory";
            // T?o m?t PlayerHistoryDataWrapper v� ??t d? li?u v�o
            Debug.Log("duration " + duration);
            Debug.Log("player " + player.id);
            Debug.Log("eventTaskId " + eventTaskId);
            Debug.Log("TaskPoint " + taskPoint);
            if (duration == 0)
            {
                Debug.Log("Vao trong nay");
                playerHistoryData = new PlayerHistoryData
                {
                    eventtaskId = eventTaskId,
                    playerId = player.id,
                    completedTime = duration,
                    taskPoint = taskPoint,
                    status = "FAILED"
                };
            }
            else
            {
                Debug.Log("Vao trong day la success");

                playerHistoryData = new PlayerHistoryData
                {
                    eventtaskId = eventTaskId,
                    playerId = player.id,
                    completedTime = duration,
                    taskPoint = taskPoint,
                    status = "SUCCESS"
                };
            }



            // Chuy?n ??i PlayerHistoryDataWrapper th�nh JSON
            string jsonData = JsonUtility.ToJson(playerHistoryData);

            // T?o m?t UnityWebRequest ?? g?i POST request
            var request = new UnityWebRequest(apiEndpoint, "POST");
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("L?i: " + request.error);
            }
            else
            {
                Debug.Log("Ph?n h?i t? server: " + request.downloadHandler.text);

                player.totalPoint += taskPoint;
                player.totalTime += duration;
                player.isplayer = true;
                string playerUrl = $"https://anhkiet-001-site1.htempurl.com/api/Players/{player.id}";
                string jsonDataPlayer = JsonUtility.ToJson(player);
                NamePrefab.Instance.PutDataPlayer(playerUrl, jsonDataPlayer);
            }
        }
        else
        {
            Debug.Log("Player bang null hoac khong lay duoc");
        }

    }
    private void OnPlayerIdReceived(Player playerData)
    {
        player = playerData;
    }

    public void NotifyTaskCompletion(string taskID, string dateTimeSuccess, double point, double pointReward)
    {
        taskDtos = EventLoader.Instance.GetAllTaskData();
        Debug.Log("Count: " + TaskManager.Instance.taskItems.Count);
        Debug.Log("TaskID: " + TaskManager.Instance.taskItems[0].taskId);
        // T�m ki?m TaskItem t??ng ?ng trong danh  s�ch taskItems
        var taskItem = TaskManager.Instance.taskItems.FirstOrDefault(item => item.taskId == taskID);
        Debug.Log("Task Item: " + taskItem);
        if (taskItem != null)
        {
            var task = taskDtos.FirstOrDefault(x => x.id == taskID);
            if (task == null)
            {
                Debug.Log("task is Null");

            }
            else
            {
                var TimeStartMission = DateTime.Parse(task.starttime).TimeOfDay;
                var endTime = DateTime.Parse(task.endtime).TimeOfDay;
                DateTime timeDateTime;


                if (DateTime.TryParseExact(dateTimeSuccess, "HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out timeDateTime))
                {
                    TimeSpan timeSuccess = timeDateTime.TimeOfDay;
                    Debug.Log("startTime" + TimeStartMission);
                    Debug.Log("Time" + timeSuccess);
                    Debug.Log("EndTime" + endTime);
                    TimeSpan durationTime = endTime - TimeStartMission;

                    TimeSpan oneThirdDuration = TimeSpan.FromTicks(durationTime.Ticks / 3);
                    TimeSpan halfDuration = TimeSpan.FromTicks(durationTime.Ticks / 2);

                    TimeSpan oneThirdAfterStart = TimeStartMission + oneThirdDuration;
                    TimeSpan halfAfterStart = TimeStartMission + halfDuration;
                    if (task.type.Equals("CHECKIN"))
                    {
                        if (timeSuccess >= TimeStartMission && timeSuccess <= endTime)
                        {
                            UITween.Instance.UiCompleteTask1.SetActive(true);

                            if (timeSuccess >= TimeStartMission && timeSuccess <= oneThirdAfterStart)
                            {
                                Debug.Log("Hoan thanh nhanh ");

                            }
                            else if (timeSuccess > oneThirdAfterStart && timeSuccess <= halfAfterStart)
                            {
                                // Handle 1/2 duration logic here
                                Debug.Log("Hoan thanh vua ");

                                UITween.Instance.star3.SetActive(false);
                            }
                            else
                            {
                                Debug.Log("Hoan thanh cham nhat ");

                                UITween.Instance.star3.SetActive(false);
                                UITween.Instance.star2.SetActive(false);

                                // Handle remaining duration logic here
                            }
                            TimeSpan duration = timeSuccess - TimeStartMission;
                            taskItem.CheckCompletion(true, duration.Minutes, task.eventtaskId, point);
                            ManageButton.Instance.AddToCoint(point);

                        }
                        else
                        {
                            Debug.Log("Khong vao duoc khong");
                            UITween.Instance.UiCompleteTask1.SetActive(true);

                            UITween.Instance.star1.SetActive(false);
                            UITween.Instance.star3.SetActive(false);
                            UITween.Instance.star2.SetActive(false);
                            Debug.Log("EventTask" + task.eventtaskId);
                            taskItem.CheckCompletion(false, 0, task.eventtaskId, 0.0);
                            ManageButton.Instance.AddToCoint(point);

                        }
                    }
                    else if (task.type.Equals("QUESTIONANDANSWER"))
                    {
                        if (timeSuccess >= TimeStartMission && timeSuccess <= endTime)
                        {
                            UITween.Instance.UiCompleteTask2.SetActive(true);

                            if (timeSuccess >= TimeStartMission && timeSuccess <= oneThirdAfterStart && pointReward == 100)
                            {
                                Debug.Log("Hoan thanh nhanh ");

                            }
                            else if (timeSuccess > oneThirdAfterStart && timeSuccess <= halfAfterStart && pointReward < 100 && pointReward >= 50)
                            {
                                // Handle 1/2 duration logic here
                                Debug.Log("Hoan thanh vua ");

                                UITween.Instance.star3.SetActive(false);
                            }
                            else
                            {
                                Debug.Log("Hoan thanh cham nhat ");

                                UITween.Instance.star3.SetActive(false);
                                UITween.Instance.star2.SetActive(false);

                                // Handle remaining duration logic here
                            }
                            TimeSpan duration = timeSuccess - TimeStartMission;
                            var totalPoint = point + pointReward;
                            taskItem.CheckCompletion(true, duration.Minutes, task.eventtaskId, totalPoint);
                            ManageButton.Instance.AddToCoint(point);

                        }
                        else
                        {
                            Debug.Log("Khong vao duoc khong");
                            UITween.Instance.UiCompleteTask2.SetActive(true);

                            UITween.Instance.star1.SetActive(false);
                            UITween.Instance.star3.SetActive(false);
                            UITween.Instance.star2.SetActive(false);
                            Debug.Log("EventTask" + task.eventtaskId);
                            TimeSpan duration = timeSuccess - TimeStartMission;

                            taskItem.CheckCompletion(false, duration.Minutes, task.eventtaskId, 0.0);
                            ManageButton.Instance.AddToCoint(point);

                        }
                    }

                }
            }

        }
        else
        {
            Debug.Log("Task Item is Null");

        }
    }




    public IEnumerator GetPlayerHistoryByTaskIdAndPlayerId(string taskId, string playerId, Action<string> callback)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/PlayerHistorys/task/{taskId}/{playerId}";
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
                PlayerHistoryDataWrapper wrapper = JsonUtility.FromJson<PlayerHistoryDataWrapper>(response);
                Debug.Log(wrapper.data.status);
                // Access the properties of majorData
                string status = wrapper.data.status;
                Debug.Log("Status: " + status);
                // Call the callback function with the majorName
                callback?.Invoke(status);
            }
            else
            {
                Debug.LogError("API call failed. Error: " + webRequest.error);
            }
        }
    }
}

