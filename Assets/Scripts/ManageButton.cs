using Photon.Pun;
using TMPro;
using UnityEngine;

public class ManageButton : MultiplayerSingleton<ManageButton>
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject mission;
    [SerializeField] private GameObject chatPannel;
    [SerializeField] private GameObject locationList;
    [SerializeField] private GameObject settingPannel;
    [SerializeField] private TMP_Text textCoins;
    [SerializeField] private TMP_Text textCointInventory;
    [SerializeField] private GameObject fixedJoystick;
    [SerializeField] private GameObject floatJoystick;
    [SerializeField] private GameObject chatInteract;

    [SerializeField] public GameObject canvasButton;
    public double point { get; set; }


    bool isOpen = false;


    private void Update()
    {
        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("CheckButton", out object signalObj);
        string check = signalObj as string;
        if (check != null)
        {
            if (check.Equals("on"))
            {
                canvasButton.gameObject.SetActive(true);
                fixedJoystick.gameObject.SetActive(true);
                floatJoystick.gameObject.SetActive(true);
            }

            if (check.Equals("off"))
            {
                canvasButton.gameObject.SetActive(false);
                fixedJoystick.gameObject.SetActive(false);
                floatJoystick.gameObject.SetActive(false);
            }
        }

        PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("chatInteract", out object signalObja);
        string check2 = signalObja as string;
        if (check2 != null)
        {
            if (check2.Equals("on"))
            {
                chatInteract.gameObject.SetActive(true);
            }

            if (check2.Equals("off"))
            {
                chatInteract.gameObject.SetActive(false);
            }
        }
    }

    private void showPoint()
    {
        double point = NamePrefab.Instance.currentPoint;
        AddToCoint(point);

    }
    // Update is called once per frame
    public void AddToCoint(double amount)
    {
        point += amount;
        textCoins.text = point.ToString();


    }
    public void UpdateToCoint(double amount)
    {
        point = amount;
        Debug.Log(point);
        textCoins.text = point.ToString();
        textCointInventory.text = point.ToString();

    }
    public void OpenAllUI()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });

    }
    public void CloseAllUI()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
    }
    public void OpenMission()
    {
        mission.gameObject.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
    }

    public void CloseMission()
    {
        mission.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

    public void OpenInventory()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        inventory.gameObject.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });

    }

    public void CloseInventory()
    {
        inventory.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });

    }

    public void OpenChat()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        chatPannel.gameObject.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
    }

    public void CloseChat()
    {
        chatPannel.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

    public void OpenLocation()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        locationList.gameObject.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
    }

    public void CloseLocation()
    {
        locationList.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }

    public void OpenSetting()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "off" } });
        settingPannel.gameObject.SetActive(true);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
    }

    public void CloseSetting()
    {
        settingPannel.gameObject.SetActive(false);
        PhotonNetwork.LocalPlayer.SetCustomProperties(
         new ExitGames.Client.Photon.Hashtable() { { "chatInteract", "aaa" } });
        PhotonNetwork.LocalPlayer.SetCustomProperties(
            new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "on" } });
    }
}
