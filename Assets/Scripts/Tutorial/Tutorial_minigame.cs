using Photon.Pun;
using UnityEngine;

public class Tutorial_minigame : MonoBehaviour
{
    public GameObject[] panels;
    public GameObject ActiveGame;
    private int currentPanelIndex = 0;

    private void Start()
    {
        ShowCurrentPanel();
    }

    public void NextButtonClick()
    {
        HideCurrentPanel();
        currentPanelIndex = (currentPanelIndex + 1) % panels.Length;
        ShowCurrentPanel();
    }

    private void HideCurrentPanel()
    {
        panels[currentPanelIndex].SetActive(false);
    }

    private void ShowCurrentPanel()
    {
        panels[currentPanelIndex].SetActive(true);
    }

    public void finalButton()
    {
        foreach (GameObject panel in panels)
        {
            panel.gameObject.SetActive(false);
        }
        ActiveGame.gameObject.SetActive(true);
    }

    public void CloseGame()
    {
        ActiveGame.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

}
