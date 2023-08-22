using System;
using TMPro;
using UnityEngine;

public class NPCQuestionAndAnswer : MonoBehaviour
{
    [SerializeField] private TMP_Text nameTxt;
    [SerializeField] public TMP_Text coinText;
    [SerializeField] private GameObject ToActive;
    [SerializeField] private GameObject EventTrigger;
    [SerializeField] private GameObject taskMission;
    public TaskDto taskType;
    public string major;
    private string location;
    private string status;

    private void Start()
    {
        nameTxt.text = taskType.name;

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

    public void QuestionAndAnswer(double pointReward)
    {
        ToActive.SetActive(false);
        DateTime currentTime = DateTime.Now;
        string formattedTime = currentTime.ToString("HH:mm:ss");
        Debug.Log(formattedTime);
        PlayerHistoryAPI.Instance.NotifyTaskCompletion(taskType.id, formattedTime, taskType.point, pointReward);
        coinText.text = taskType.point.ToString() + pointReward;
        ManageButton.Instance.CloseAllUI();

    }

    public void ClaimMoney()
    {
        ManageButton.Instance.OpenAllUI();
        EventTrigger.SetActive(false);
        taskMission.SetActive(false);
    }
}
