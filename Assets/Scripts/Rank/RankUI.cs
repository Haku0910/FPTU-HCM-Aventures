using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RankUI : MultiplayerSingleton<RankUI>
{
    public List<RankData> rankDataList = new List<RankData>();

    [SerializeField] GameObject PlayerRankTemplate;
    [SerializeField] GameObject rankPanel;

    GameObject g;
    [SerializeField] Transform ScrollView;
    [SerializeField] Sprite top1RankIcon;
    [SerializeField] Sprite top2RankIcon;
    [SerializeField] Sprite top3RankIcon;

    private void Start()
    {

        float updateInterval = 30.0f; // C?p nh?t rank m?i 30 giây (có th? thay ??i)
        StartCoroutine(UpdateRankDataPeriodically(updateInterval));
    }
    private IEnumerator UpdateRankDataPeriodically(float updateInterval)
    {
        while (true)
        {
            // G?i hàm c?p nh?t d? li?u rank ? ?ây
            ShowAllPlayersRank(10);

            yield return new WaitForSeconds(updateInterval);
        }
    }


    private void ShowAllPlayersRank(int countToShow)
    {
        int count = Mathf.Min(countToShow, rankDataList.Count);

        int counter = rankDataList.Count;
        Debug.Log("Count" + counter);
        if (counter > 0)
        {
            if (counter > count)
            {
                for (int i = 0; i < count; i++)
                {
                    g = Instantiate(PlayerRankTemplate, ScrollView);
                    if (i < 3)
                    {
                        Sprite rankIcon = null;
                        if (i == 0)
                        {
                            rankIcon = top1RankIcon;
                        }
                        else if (i == 1)
                        {
                            rankIcon = top2RankIcon;
                        }
                        else if (i == 2)
                        {
                            rankIcon = top3RankIcon;
                        }
                        g.transform.GetChild(0).GetComponent<Image>().sprite = rankIcon;
                    }
                    else
                    {
                        g.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
                    }
                    g.transform.GetChild(3).GetComponent<TMP_Text>().text = rankDataList[i].nickname;
                    g.transform.GetChild(4).GetComponent<TMP_Text>().text = rankDataList[i].totalPoint.ToString();
                }
            }
            else
            {
                for (int i = 0; i < counter; i++)
                {
                    g = Instantiate(PlayerRankTemplate, ScrollView);
                    if (i < 3)
                    {
                        Sprite rankIcon = null;
                        if (i == 0)
                        {
                            rankIcon = top1RankIcon;
                        }
                        else if (i == 1)
                        {
                            rankIcon = top2RankIcon;
                        }
                        else if (i == 2)
                        {
                            rankIcon = top3RankIcon;
                        }
                        g.transform.GetChild(0).GetComponent<Image>().sprite = rankIcon;
                    }
                    else
                    {
                        g.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
                    }
                    g.transform.GetChild(3).GetComponent<TMP_Text>().text = rankDataList[i].nickname;
                    g.transform.GetChild(4).GetComponent<TMP_Text>().text = rankDataList[i].totalPoint.ToString();

                }
            }
        }
        else
        {
            Debug.Log("Null");
        }
    }

    public void OpenRank()
    {
        rankPanel.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });

    }

    public void CloseRank()
    {
        rankPanel.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }
}
