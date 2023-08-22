using UnityEngine;

public class PlatformDetector : MonoBehaviour
{
    [SerializeField] private GameObject joytick;
    private bool isMobilePlatform;


    private void Awake()
    {
        // Kiểm tra nếu ứng dụng đang chạy trên nền tảng di động
#if UNITY_ANDROID || UNITY_IOS
        isMobilePlatform = true;
#else
        isMobilePlatform = false;
#endif

        // In ra thông báo tương ứng
        if (isMobilePlatform)
        {
            joytick.gameObject.SetActive(true);
            Debug.Log("Ứng dụng đang chạy trên thiết bị di động.");
        }
        else
        {
            joytick.gameObject.SetActive(false);
            Debug.Log("Ứng dụng đang chạy trên PC.");
        }
    }

    public bool PlatformCheck()
    {
        if (isMobilePlatform)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
