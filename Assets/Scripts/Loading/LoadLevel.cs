using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLevel : MonoBehaviourPunCallbacks
{
    public GameObject loadingScreen;
    public Slider loadingSlider;
    public TMP_Text loadingText;
    public static LoadLevel Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadLevels(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        loadingScreen.SetActive(true);

        PhotonNetwork.AutomaticallySyncScene = false;

        // Load the new scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the new scene is fully loaded
        while (!PhotonNetwork.IsConnectedAndReady || !asyncLoad.isDone)
        {
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f); // Normalize the progress value
            loadingSlider.value = progress;
            loadingText.text = (progress * 100f).ToString("F0") + "%";

            yield return null;
        }

        PhotonNetwork.AutomaticallySyncScene = true;

        loadingScreen.SetActive(false);
    }
}
