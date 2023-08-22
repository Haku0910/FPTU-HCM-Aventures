using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TaskManager : MultiplayerSingleton<TaskManager>
{
    private void Awake()
    {
        StartCoroutine(LoadEventData());
    }

    private List<TaskItemData> taskItemData = new List<TaskItemData>();
    public GameObject taskPrefab;
    public NPCManager nPCManager;
    public TutorialHand tutorialHanh;
    public Transform taskParent;
    public GameObject buttonUIMission;
    public GameObject UIMission;
    public List<TaskItem> taskItems = new List<TaskItem>();
    private List<TaskDto> taskDtos;
    public EventLoader eventLoader;
    public MissionDetail missionDetail;
    public TMP_Text coins;
    private string playerId;
    public string status;
    public bool checkClaimMission = false;
    Button buyBtn;
    public Button DetailBtn;
    private const string DetailBtnMissionKey = "DetailBtnMission_";
    public GameObject notificatePanel;
    public TMP_Text notificateText;

    private void OnGetPlayer(string playerDataId)
    {
        playerId = playerDataId;
    }

    private void Start()
    {
        UIMission.SetActive(false);
    }
    private IEnumerator LoadEventData()
    {
        // ??i cho ??n khi d? li?u ???c t?i xong t? EventLoader
        while (!EventLoader.Instance.IsDataLoaded())
        {
            yield return null;
        }
        yield return StartCoroutine(NamePrefab.Instance.CheckPlayerByEmail(OnGetPlayer));

        // D? li?u ?� t?i xong, ti?p t?c x? l� d? li?u ? ?�y
        taskDtos = EventLoader.Instance.GetAllTaskData();
        coins.text = NamePrefab.Instance.currentPoint.ToString();
        StartCoroutine(GetDataForTaskItemData());

    }
    public void UIMissions()
    {
        tutorialHanh.CloseTutorial();
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        UIMission.SetActive(true);

    }

    public void recover()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
        tutorialHanh.CloseTutorial();
        UIMission.SetActive(false);
    }
    private IEnumerator GetDataForTaskItemData()
    {
        if (taskDtos != null)
        {
            if (taskItemData.Count != taskDtos.Count)
            {
                taskItemData.Clear();
                for (int i = 0; i < taskDtos.Count; i++)
                {
                    taskItemData.Add(new TaskItemData());
                }
            }
            for (int i = 0; i < taskDtos.Count; i++)
            {
                taskItemData[i].type = taskDtos[i].type;
                taskItemData[i].taskText = taskDtos[i].name;
                taskItemData[i].rewardText = taskDtos[i].point.ToString();
                taskItemData[i].NPCNameText = taskDtos[i].npcName;
                taskItemData[i].LocationNameText = taskDtos[i].locationName;
                taskItemData[i].startTime = taskDtos[i].starttime;
                taskItemData[i].endTime = taskDtos[i].endtime;
                taskItemData[i].id = taskDtos[i].id;
                taskItemData[i].eventtaskId = taskDtos[i].eventtaskId;
                taskItemData[i].point = taskDtos[i].point;
                taskItemData[i].majorName = taskDtos[i].majorName;
                taskItemData[i].majorId = taskDtos[i].majorId;
                yield return StartCoroutine(LoadStatus(taskItemData[i]));
                Debug.Log("Start time : " + taskDtos[i].starttime);
                Debug.Log("End time : " + taskDtos[i].endtime);
                Debug.Log("point : " + taskDtos[i].point);
                if (taskItemData[i] != null)
                {
                    if (taskItemData[i].type.Equals("CHECKIN"))
                    {
                        Debug.Log("Vao khong mission");
                        var location = LoadAvatar.instance.locationDatas.Where(x => x.locationName == taskItemData[i].LocationNameText).FirstOrDefault();
                        if (location != null)
                        {
                            float x = (float)location.x;
                            float y = (float)location.y;
                            float z = (float)location.z;
                            Debug.Log("X:" + x + " y: " + y + "z:" + z + "location x y z" + location.x + location.y + location.z + "TaskDtos" + taskDtos[i].name);

                            nPCManager.CreateCheckinNPC(x, y, z, Quaternion.identity, status, taskDtos[i], taskItemData[i].majorName, taskItemData[i].LocationNameText);

                        }
                    }
                    else if (taskItemData[i].type.Equals("QUESTIONANDANSWER"))
                    {
                        Debug.Log("Vao khong mission question");
                        var location = LoadAvatar.instance.locationDatas.Where(x => x.locationName == taskItemData[i].LocationNameText).FirstOrDefault();
                        if (location != null)
                        {
                            float x = (float)location.x;
                            float y = (float)location.y;
                            float z = (float)location.z;
                            Debug.Log("X:" + x + " y: " + y + "z:" + z + "location x y z" + location.x + location.y + location.z);

                            nPCManager.CreateQuestionNPC(x, y, z, Quaternion.identity, status, taskDtos[i], taskItemData[i].majorId, taskItemData[i].LocationNameText);

                        }
                    }
                }
                else
                {
                    Debug.Log("taskItemData " + i + " is null");
                }

            }
            StartCoroutine(LoadTasks());
        }
        else
        {
            Debug.Log("Task dto null");
        }

    }

    private IEnumerator LoadTasks()
    {

        foreach (Transform child in taskParent)
        {
            Destroy(child.gameObject);
        }

        if (taskDtos != null)
        {
            for (int i = 0; i < taskDtos.Count; i++)
            {
                GameObject taskObject = Instantiate(taskPrefab, taskParent);
                TaskItem taskItem = taskObject.GetComponent<TaskItem>();
                Debug.Log("Id cua task" + taskItemData[i].id);
                yield return StartCoroutine(LoadStatus(taskItemData[i]));
                Debug.Log("Status : " + status);

                taskItem.Initialize(taskItemData[i], this, missionDetail, status);
                taskItems.Add(taskItem);
                Debug.Log("Task Id" + taskDtos[i].id);
                if (status == null)
                {
                    Transform buttonTransform = taskObject.transform.Find("Button_Claim_Green");
                    if (buttonTransform != null)
                    {
                        DetailBtn = buttonTransform.GetComponent<Button>();

                        if (DetailBtn != null)
                        {
                            string taskId = taskItemData[i].id;

                            if (PlayerPrefs.HasKey(DetailBtnMissionKey + taskId))
                            {
                                checkClaimMission = true;
                                Debug.Log("DetailBtn đang bị ẩn hoặc không hoạt động.");
                                DetailBtn.gameObject.SetActive(false);

                            }
                            else
                            {
                                checkClaimMission = false;
                                DetailBtn.gameObject.SetActive(true);

                                DetailBtn.AddEventListener(i, OnDetailMissionBtnClicked);

                            }
                            buyBtn = taskObject.transform.GetComponent<Button>();

                            buyBtn.AddEventListener(i, OnMissionBtnClicked);
                        }
                    }
                }
                else
                {
                    if (status.Equals("SUCCESS") || status.Equals("FAILED"))
                    {
                        Transform buttonTransform = taskObject.transform.Find("Button_Claim_Green");
                        if (buttonTransform != null)
                        {
                            DetailBtn = buttonTransform.GetComponent<Button>();
                            DetailBtn.gameObject.SetActive(false);
                        }
                        buyBtn = taskObject.transform.GetComponent<Button>();

                        buyBtn.AddEventListener(i, OnMissionBtnClicked);
                    }
                    else
                    {

                        Transform buttonTransform = taskObject.transform.Find("Button_Claim_Green");
                        if (buttonTransform != null)
                        {
                            DetailBtn = buttonTransform.GetComponent<Button>();

                            if (DetailBtn != null)
                            {
                                string taskId = taskItemData[i].id;

                                if (PlayerPrefs.HasKey(DetailBtnMissionKey + taskId))
                                {
                                    checkClaimMission = true;

                                    Debug.Log("DetailBtn đang bị ẩn hoặc không hoạt động.");
                                    DetailBtn.gameObject.SetActive(false);

                                }
                                else
                                {
                                    checkClaimMission = false;

                                    Debug.Log("DetailBtn đang hiển thị và hoạt động.");
                                    DetailBtn.gameObject.SetActive(true);

                                    DetailBtn.AddEventListener(i, OnDetailMissionBtnClicked);

                                }

                                buyBtn = taskObject.transform.GetComponent<Button>();

                                buyBtn.AddEventListener(i, OnMissionBtnClicked);
                            }

                        }
                    }
                }

            }
        }
        else
        {
            Debug.Log("task dto is null");
        }
    }
    private IEnumerator LoadStatus(TaskItemData taskItemData)
    {
        yield return StartCoroutine(PlayerHistoryAPI.Instance.GetPlayerHistoryByTaskIdAndPlayerId(taskItemData.id, playerId, OnStatus));
    }

    private void OnStatus(string Status)
    {
        status = Status;
    }
    void OnDetailMissionBtnClicked(int itemIndex)
    {
        TimeSpan startime = TimeSpan.Parse(taskDtos[itemIndex].starttime);
        TimeSpan endtime = TimeSpan.Parse(taskDtos[itemIndex].endtime);
        TimeZoneInfo vnTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");

        // Lấy thời gian hiện tại của Việt Nam
        DateTimeOffset currentTimeVN = TimeZoneInfo.ConvertTime(DateTime.UtcNow, vnTimeZone);

        // Tính khoảng thời gian giữa thời gian hiện tại (UTC) và thời gian tại Việt Nam
        TimeSpan currentime = currentTimeVN.TimeOfDay;

        Debug.Log("start time: " + startime);
        Debug.Log("end time: " + endtime);
        Debug.Log("current time: " + currentime);

        if (startime > currentime)
        {
            //hiện pop up báo chưa tới giờ
            notificatePanel.gameObject.SetActive(true);
            notificateText.text = "Sự kiện chưa diễn ra";
        }
        else if (endtime < currentime)
        {
            //hiện pop up báo quá giờ nhận quest
            notificatePanel.gameObject.SetActive(true);
            notificateText.text = "Đã quá thời gian diễn ra sự kiện";
        }
        else
        {
            checkClaimMission = true;

            MissionDetail.Instance.ShowDetail(taskDtos[itemIndex], status);
            DetailBtn.gameObject.SetActive(false);
            PlayerPrefs.SetInt(DetailBtnMissionKey + taskDtos[itemIndex].id, 1);
        }
    }

    public void CloseNotification()
    {
        notificatePanel.gameObject.SetActive(false);
    }

    void OnMissionBtnClicked(int itemIndex)
    {
        checkClaimMission = false;
        MissionDetail.Instance.ShowDetail(taskDtos[itemIndex], status);

    }

    public IEnumerator CheckTaskById(string taskId, Action<string> callback)
    {
        string url = $"http://anhkiet-001-site1.htempurl.com/api/Tasks/{taskId}";
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
                TaskDataWrapperDetail taskDataWrapper = JsonUtility.FromJson<TaskDataWrapperDetail>(response);
                if (taskDataWrapper != null && taskDataWrapper.data.id != null)
                {
                    callback?.Invoke(taskDataWrapper.data.id);

                }
            }
            else
            {
                Debug.LogError("API call failed. Error: " + request.error);
            }
        }
    }

}