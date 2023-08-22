using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class InventoryApi : MultiplayerSingleton<InventoryApi>
{
    public void PostDataInventory(string jsonData)
    {
        StartCoroutine(PostRequest(jsonData));
    }
    public IEnumerator PostRequest(string jsonData)
    {
        var request = new UnityWebRequest("http://anhkiet-001-site1.htempurl.com/api/Inventorys/inventory", "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        string authToken = PlayerPrefs.GetString("token");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("L?i: " + request.error);
        }
        else
        {
            Debug.Log("Ph?n h?i t? server: " + request.downloadHandler.text);
        }
    }
}
