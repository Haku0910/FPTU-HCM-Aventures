using System;
using TMPro;
using UnityEngine;

public class CheckInNpc : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] public TMP_Text coinText;
    [SerializeField] public GameObject textError;
    [SerializeField] private GameObject taskMission;
    [SerializeField] private GameObject ToActive;
    [SerializeField] private GameObject EventTrigger;
    [SerializeField] private ButtonManager button;
    private TaskDto taskType;
    private string major;
    private string location;
    private string status;
    private void Start()
    {
        if (status != null)
        {
            if (status.Equals("SUCCESS") || status.Equals("FAILED"))
            {
                EventTrigger.SetActive(false);

            }
        }
    }
    public void SetTaskDto(TaskDto taskType)
    {
        this.taskType = taskType;
    }

    public void SetStatus(string Status)
    {
        this.status = Status;
    }
    public void SetMajor(string major)
    {
        this.major = major;
    }

    public void SetLocation(string location)
    {
        this.location = location;
    }

    public void CheckIn()
    {
        if (TaskManager.Instance.DetailBtn.gameObject.activeSelf == false)
        {
            ManageButton.Instance.OpenMission();
            EventTrigger.SetActive(false);
            ToActive.SetActive(false);
            DateTime currentTime = DateTime.Now;
            string formattedTime = currentTime.ToString("HH:mm:ss");
            Debug.Log(formattedTime);
            ManageButton.Instance.CloseAllUI();
            PlayerHistoryAPI.Instance.NotifyTaskCompletion(taskType.id, formattedTime, taskType.point, 0);
        }
        else
        {
            textError.SetActive(true);
        }

    }
    public void CloseTextError()
    {
        textError.SetActive(false);
    }

    public void ClaimMoney()
    {
        ManageButton.Instance.OpenAllUI();

        taskMission.SetActive(false);
    }
}
