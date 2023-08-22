using UnityEngine;
using UnityEngine.Video;

public class EndSwitch : MonoBehaviour
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
        Debug.Log("Contact Switch Stop");
        StopSwitchFunction();
        return true;
    }

    public void StopSwitchFunction()
    {
        videoPlayer.Stop();
    }
}
