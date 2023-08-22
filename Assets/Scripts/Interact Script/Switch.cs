using UnityEngine;
using UnityEngine.Video;

public class Switch : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt;
    [SerializeField] private string videoURL;  // Đường dẫn URL của video trực tuyến
    [SerializeField] private VideoPlayer videoPlayer;

    private void Start()
    {
        VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
    }

    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Contact Switch");
        StartSwitch();
        return true;
    }

    public void StartSwitch()
    {

        videoPlayer.url = Application.streamingAssetsPath + "/Vài Giây Nữa Thôi.mp4";
        videoPlayer.Prepare();

        // Bắt đầu tải và phát video
        videoPlayer.Prepare();
        videoPlayer.Play();
    }
}
