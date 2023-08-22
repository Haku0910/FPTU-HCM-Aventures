using UnityEngine;

[CreateAssetMenu(fileName = "Mission", menuName = "Event/Task")]

public class TaskItemData : ScriptableObject
{
    public string id;
    public string eventtaskId;
    public string majorId;
    public string type;
    public string taskText;
    public string rewardText;
    public string timeStartAndEndText;
    public string LocationNameText;
    public string NPCNameText;
    public string startTime;
    public string endTime;
    public double point;
    public string majorName;

}
