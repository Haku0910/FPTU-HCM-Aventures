using UnityEngine;
using UnityEngine.Video;

public class StartSwitch : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt;
    [SerializeField] private string videoURL;  // Đường dẫn URL của video trực tuyến
    [SerializeField] private VideoPlayer videoPlayer;
    private bool isVideoPlaying = false;

    private void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
    }

    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Contact Switch");
        StartSwitchFunction();
        return true;
    }

    public void StartSwitchFunction()
    {

        videoPlayer.url = Application.streamingAssetsPath + "/Vài Giây Nữa Thôi.mp4";
        videoPlayer.Prepare();

        if (isVideoPlaying)
        {
            // Nếu video đang phát, dừng video lại
            videoPlayer.Stop();
            isVideoPlaying = false;
        }
        else
        {
            // Nếu video không phát, bắt đầu phát video
            videoPlayer.Play();
            isVideoPlaying = true;
        }
    }
}
