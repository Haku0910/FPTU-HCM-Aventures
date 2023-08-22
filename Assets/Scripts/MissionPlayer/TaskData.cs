using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class TaskData
{
    public string id;
    public string locationId;
    public string majorId;
    public string npcId;
    public string endTime;
    public bool isRequireitem;
    public int timeOutAmount;
    public string activityName;
    public int point;
    public string type;
    public string status;
}
