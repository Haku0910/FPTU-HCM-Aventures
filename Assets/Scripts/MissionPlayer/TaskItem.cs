using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskItem : MonoBehaviour
{
    public string taskId;
    public TMP_Text type;
    public TMP_Text taskText;
    public TMP_Text rewardText;
    public Image errorFailedIcon;
    public Image completedIcon;
    private TaskManager taskManager;
    private TaskItemData TaskItemData;

    // G?i ph??ng th?c n�y khi kh?i t?o TaskItem t? prefab

    public void Initialize(TaskItemData data, TaskManager manager, MissionDetail missionDetail, string status)
    {
        TaskItemData = data;
        taskId = data.id;
        taskManager = manager;
        missionDetail.LocationNameText.text = data.LocationNameText;
        missionDetail.NPCNameText.text = data.NPCNameText;
        taskText.text = data.taskText;
        type.text = data.type;
        rewardText.text = data.point.ToString();

        // Kiểm tra trạng thái và hiển thị biểu tượng phù hợp

        if (status == null)
        {
            completedIcon.gameObject.SetActive(false);
            errorFailedIcon.gameObject.SetActive(false);
        }
        else
        {
            if (status.Equals("SUCCESS"))
            {
                Debug.Log("1 la vao success");
                errorFailedIcon.gameObject.SetActive(false);
                completedIcon.gameObject.SetActive(true);
            }
            else if (status.Equals("FAILED"))
            {
                Debug.Log("2 la vao failed");

                completedIcon.gameObject.SetActive(false);
                errorFailedIcon.gameObject.SetActive(true);
            }
            else
            {
                Debug.Log("3 la vao ");

                completedIcon.gameObject.SetActive(false);
                errorFailedIcon.gameObject.SetActive(false);
            }
        }


    }
    public void CheckCompletion(bool IsSuccess, double duration, string eventTaskId, double point)
    {
        if (IsSuccess == true)
        {
            // Hi?n th? bi?u t??ng ho�n th�nh nhi?m v?
            // V� d?: completedIcon.SetActive(true);
            CompleteTask(eventTaskId, point, duration);

            completedIcon.gameObject.SetActive(true);
            errorFailedIcon.gameObject.SetActive(false);
        }
        else
        {
            CompleteTask(eventTaskId, point, duration);

            completedIcon.gameObject.SetActive(false);

            errorFailedIcon.gameObject.SetActive(true);

        }
    }
    public void CompleteTask(string eventTaskId, double point, double duration)
    {
        PlayerHistoryAPI.Instance.CheckTaskCompletionUpdate(eventTaskId, point, duration);
    }


}
