using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MissionDetail : MultiplayerSingleton<MissionDetail>
{
    public TMP_Text timeEndText;
    public TMP_Text durationText;
    public TMP_Text LocationNameText;
    public TMP_Text NPCNameText;
    public TMP_Text coinsText;
    public Button getCoinBtn;
    private TimeSpan startTime;
    private TimeSpan endTime;
    private TimeSpan timeDifference;
    private bool isCountingDown = false;



    private void Start()
    {
        this.gameObject.SetActive(false);
    }

    public void ShowDetail(TaskDto data, string status)
    {

        this.gameObject.SetActive(true);
        startTime = TimeSpan.Parse(data.starttime);
        endTime = TimeSpan.Parse(data.endtime);
        timeDifference = endTime - startTime;
        Debug.Log("Check :" + TaskManager.Instance.checkClaimMission);
        string Check = null;
        if (TaskManager.Instance.checkClaimMission)
        {
            isCountingDown = true; // B?t ??u ??m ng??c
            StartCoroutine(UpdateTimer(status));
            Check = FormatTimeDifference(timeDifference);
            durationText.text = Check;
        }
        else
        {
            if (Check == null)
            {
            }
            else
            {
                durationText.text = Check;
            }
        }
        timeEndText.text = endTime.ToString();

        LocationNameText.text = data.locationName;
        NPCNameText.text = data.npcName;
        coinsText.text = data.point.ToString();
    }



    private IEnumerator UpdateTimer(string status)
    {
        while (timeDifference.TotalSeconds > 0)
        {
            if (!isCountingDown)
            {
                yield break; // D?ng ??ng h? ??m ng??c n?u kh�ng ??m
            }
            if (status == "SUCCESS" || status == "FAILED")
            {
                durationText.text = FormatTimeDifference(timeDifference); // L?u th?i gian d?ng l?i
                yield break; // D?ng ??m ng??c n?u tr?ng th�i l� SUCCESS
            }
            timeDifference = timeDifference.Subtract(TimeSpan.FromSeconds(1));
            durationText.text = FormatTimeDifference(timeDifference);

            yield return new WaitForSeconds(1f);
        }

        durationText.text = "Expired";
        isCountingDown = false;
    }
    private string FormatTimeDifference(TimeSpan time)
    {
        return string.Format("{0:D2}:{1:D2}:{2:D2}", time.Hours, time.Minutes, time.Seconds);
    }
}
