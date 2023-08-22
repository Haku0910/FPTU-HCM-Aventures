using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;

public class CustomPropertiesManager : MonoBehaviourPunCallbacks
{
    // Tạo một đối tượng static để lưu trữ CustomProperties dựa trên UserID
    private static Dictionary<string, Hashtable> customProperties = new Dictionary<string, Hashtable>();

    private void Awake()
    {
        // Đảm bảo chỉ có một CustomPropertiesManager tồn tại trong scene
        if (FindObjectsOfType<CustomPropertiesManager>().Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            // Giữ CustomPropertiesManager tồn tại qua các scene
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void SetCustomProperties(string userId, Hashtable properties)
    {
        customProperties[userId] = properties;
    }

    public static Hashtable GetCustomProperties(string userId)
    {
        if (customProperties.ContainsKey(userId))
        {
            return customProperties[userId];
        }
        else
        {
            return null;
        }
    }
}