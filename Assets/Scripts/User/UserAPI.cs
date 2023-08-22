using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UserAPI : MonoBehaviour
{
    private const string userEndpoint = "https://649eedf8245f077f3e9d1ff9.mockapi.io/user";

    public IEnumerator GetUser(string userId, System.Action<User> callback)
    {
        string url = userEndpoint + userId;
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            www.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.LogError("Error: " + www.error);
            }
            else
            {
                string json = www.downloadHandler.text;
                User userData = JsonUtility.FromJson<User>(json);
                callback?.Invoke(userData);
            }
        }
    }

    public IEnumerator IsPlayer(string userId, System.Action<bool> callback)
    {
        string url = "https://649eedf8245f077f3e9d1ff9.mockapi.io/user/" + userId + "/player";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            string authToken = PlayerPrefs.GetString("token");
            www.SetRequestHeader("Authorization", "Bearer " + authToken);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error checking if user is player: " + www.error);
                callback.Invoke(false);
            }
            else
            {
                bool isPlayer = !string.IsNullOrEmpty(www.downloadHandler.text);
                callback.Invoke(isPlayer);
            }
        }
    }

}
