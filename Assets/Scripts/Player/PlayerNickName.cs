using Photon.Pun;
using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerNickName : MonoBehaviour
{
    private int playerScore;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text errorText;
    [SerializeField] private GameObject errorGame;
    private string playerNickName;
    private string dataPlayerId;
    // Start is called before the first frame update
    void Start()
    {
        playerNickName = PhotonNetwork.LocalPlayer.NickName;
        Debug.Log("Player nickname: " + playerNickName);
    }
    public void EnterFieldText()
    {
        if (string.IsNullOrEmpty(inputField.text))
        {
            errorGame.SetActive(true);
            errorText.text = "Nickname cannot be empty!";
            return; // Ngừng hoạt động nếu nhập sai
        }
        StartCoroutine(CheckUserByEmailToUpdatePoint(playerNickName));

    }
    public IEnumerator CheckUserByEmailToUpdatePoint(string studentId)
    {
        string url = $"https://anhkiet-001-site1.htempurl.com/api/Players/user/{studentId}";
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
              string authToken = PlayerPrefs.GetString("token");
              request.SetRequestHeader("Authorization", "Bearer " + authToken);
            // G?i yêu c?u và ch? ph?n h?i t? API
            yield return request.SendWebRequest();

            // Ki?m tra ph?n h?i t? API
            if (request.result == UnityWebRequest.Result.Success)
            {
                string response = request.downloadHandler.text;

                // Phân tích ph?n h?i t? API ?? xác ??nh ng??i dùng
                PlayerDataWrapper playerDataWrapper = JsonUtility.FromJson<PlayerDataWrapper>(response);

                Debug.Log(QuestionDialog.scoreValue);
                Debug.Log(inputField.text);
                if (playerDataWrapper != null)
                {
                    string playerNickNameUrl = $"https://anhkiet-001-site1.htempurl.com/api/Players/player/{inputField.text}";
                    using (UnityWebRequest playerNickNameRequest = UnityWebRequest.Get(playerNickNameUrl))
                    {
                        /*                        playerNickNameRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
                        */
                        yield return playerNickNameRequest.SendWebRequest();

                        if (playerNickNameRequest.result == UnityWebRequest.Result.Success)
                        {
                            string playerNickResponse = playerNickNameRequest.downloadHandler.text;
                            PlayerDataWrapper playerDataNickWrapper = JsonUtility.FromJson<PlayerDataWrapper>(playerNickResponse);
                            Debug.Log(playerDataNickWrapper.data.id);
                            if (playerDataNickWrapper.data.nickname == null)
                            {
                                Debug.Log("Vao khong nguoi oi");
                                playerDataWrapper.data.nickname = inputField.text;
                                playerDataWrapper.data.totalPoint = QuestionDialog.scoreValue;
                                playerDataWrapper.data.isplayer = true;
                                string jsonString = JsonUtility.ToJson(playerDataWrapper.data);
                                PhotonNetwork.NickName = playerDataWrapper.data.nickname;
                                Debug.Log("id player" + playerDataWrapper.data.id);
                                dataPlayerId = playerDataWrapper.data.id;
                                string playerUpdateUrl = $"https://anhkiet-001-site1.htempurl.com/api/Players/{playerDataWrapper.data.id}";
                                StartCoroutine(PutRequest(playerUpdateUrl, jsonString));
                            }
                            else
                            {
                                errorGame.SetActive(true);
                                errorText.text = "Nick name bị trùng";
                                yield break;
                            }
                        }
                        else
                        {
                            // X?y ra l?i khi g?i yêu c?u API l?y thông tin ng??i ch?i
                            Debug.Log("L?i khi g?i yêu c?u API: " + playerNickNameRequest.error);
                            yield break;

                        }
                    }
                }
                else
                {
                    Debug.Log("Student không t?n t?i");
                    yield break;

                }
            }
            else
            {
                // X?y ra l?i khi g?i yêu c?u API
                Debug.Log("L?i khi g?i yêu c?u API: " + request.error);
                yield break;

            }
        }

        // Ki?m tra isInputValid và g?i l?i hàm n?u thông tin không ?úng

    }

    public IEnumerator PutRequest(string playerUrl, string jsonData)
    {
        var request = new UnityWebRequest(playerUrl, "PUT");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        string authToken = PlayerPrefs.GetString("token");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("L?i: " + request.error);
        }
        else
        {
            Debug.Log("Ph?n h?i t? server: " + request.downloadHandler.text);

            if (dataPlayerId != null)
            {
                string playerId = dataPlayerId;
                Debug.Log(playerId);
                if (playerId != null)
                {
                    var inventory = new InventoryData()
                    {
                        playerId = playerId
                    };
                    string jsonStringData = JsonUtility.ToJson(inventory);
                    StartCoroutine(PostInventory("https://anhkiet-001-site1.htempurl.com/api/Inventorys/inventory", jsonStringData));
                }
                else
                {
                    Debug.LogError("playerDataWrapper.data.id là null ho?c không h?p l?.");
                }



            }

        }
    }
    public IEnumerator PostInventory(string inventoryUrl, string jsonData)
    {
        var request = new UnityWebRequest(inventoryUrl, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        string authToken = PlayerPrefs.GetString("token");
        request.SetRequestHeader("Authorization", "Bearer " + authToken);
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("L?i khi g?i yêu c?u API: " + request.error);
            Debug.LogError("Ph?n h?i t? server: " + request.downloadHandler.text); // In ra ph?n h?i t? server ?? xem thông báo l?i chi ti?t.
        }
        else
        {
            Debug.Log("Ph?n h?i t? server (Inventory): " + request.downloadHandler.text);
            PhotonNetwork.LoadLevel("Playground1");
        }
    }
}
