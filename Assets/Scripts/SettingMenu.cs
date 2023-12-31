using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviourPunCallbacks
{
    public Slider volumeSlider;
    public GameObject pannel;

    private void Start()
    {
        pannel.gameObject.SetActive(false);
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", (float)0.5);
            Load();
        }
        else
        {
            PlayerPrefs.SetFloat("musicVolume", (float)0.5);
            Load();
        }
    }

    public void setVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", volumeSlider.value);
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void LogoutApi()
    {
        StartCoroutine(SendDeleteRequest());
    }

    private IEnumerator SendDeleteRequest()
    {
        string url = "https://anhkiet-001-site1.htempurl.com/api/Accounts/logout";

        using (UnityWebRequest request = UnityWebRequest.Delete(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            request.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log("Delete request failed. Error: " + request.error);
            }
            else
            {
                Debug.Log("Delete request successful");
                if (PhotonNetwork.IsConnected)
                {
                    if (PhotonNetwork.InRoom)
                    {

                        PhotonNetwork.LeaveRoom(); // Leave the current room
                    }

                    PhotonNetwork.Disconnect();
                    Debug.Log("Photon disconnect initiated");

                    // Disconnect from Photon servers
                }
                SceneManager.LoadSceneAsync("LoginScene");
            }
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);

        Debug.Log("Logout successful");

        PhotonNetwork.Disconnect();
    }
}
