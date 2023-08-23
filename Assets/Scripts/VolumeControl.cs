using UnityEngine;

public class VolumeControl : MonoBehaviour
{

    [SerializeField] private GameObject muteButton;
    [SerializeField] private GameObject UnmuteButton;

    void Start()
    {
        UnmuteButton.gameObject.SetActive(true);
        muteButton.gameObject.SetActive(false);
        PlayerPrefs.SetFloat("musicVolume", 1);
    }

    private void FixedUpdate()
    {
        float value = PlayerPrefs.GetFloat("musicVolume");
        AudioListener.volume = value;
    }
    public void Mute()
    {
        PlayerPrefs.SetFloat("musicVolume", 1);
        UnmuteButton.gameObject.SetActive(true);
        muteButton.gameObject.SetActive(false);
    }
    public void Unmute()
    {
        PlayerPrefs.SetFloat("musicVolume", 0);
        UnmuteButton.gameObject.SetActive(false);
        muteButton.gameObject.SetActive(true);
    }
}
