using ExitGames.Client.Photon;
using Photon.Chat;
using Photon.Pun;
using System;
using TMPro;
using UnityEngine;

public class PhotonChatManager : MonoBehaviour, IChatClientListener
{
    ChatClient chatClient;
    bool isConnected;
    [SerializeField] private string username;

    [SerializeField] private GameObject chatPannel;
    string privateReceiver = "";
    string currentChat;
    [SerializeField] private TMP_InputField chatField;
    [SerializeField] private TMP_Text chatDisplay;
    private string textUserId, textData;
    [SerializeField] GameCoins gameCoins;
    /*[SerializeField] private TMP_Text chatName;
    [SerializeField] private TMP_Text chatMess;*/

    // Start is called before the first frame update
    /*private void Awake()
    {
        username = PhotonNetwork.LocalPlayer.NickName;
        chatClient = new ChatClient(this);
        Debug.Log("Nick: " + username);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
    }*/

    void Update()
    {
        if (isConnected)
        {
            chatClient.Service();
        }

        if (chatField.text != "" && Input.GetKey(KeyCode.Return))
        {
            SubmitPublicChatOnClick();
        }
    }

    public void UsernameOnValueChange(string value)
    {
        username = value;
    }

    public void ChatConnectOnClick()
    {
        username = PhotonNetwork.LocalPlayer.NickName;
        isConnected = true;
        chatClient = new ChatClient(this);
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion, new AuthenticationValues(username));
        Debug.Log("Connect Chat ");
    }

    public void TypeChatOnValueChange(string value)
    {
        currentChat = value;
    }

    public void SubmitPublicChatOnClick()
    {
        string command = chatField.text.Substring(0, 4);
        string a = "/add";
        bool check = command.Equals(a, StringComparison.OrdinalIgnoreCase);
        if (check)
        {
            AddMoney();
        }

        if (privateReceiver == "")
        {
            chatClient.PublishMessage("FPT Channel", currentChat);
            chatField.text = "";
            currentChat = "";
        }
    }

    //only admin use for testing add money 
    private void AddMoney()
    {
        GameObject find711 = GameObject.Find("711");
        if (find711 != null)
        {
            Transform findAPI = find711.transform.Find("API");
            if (findAPI != null)
            {
                Transform gamecoin = findAPI.transform.Find("Coins");
                if (gamecoin != null)
                {
                    gameCoins = gamecoin.GetComponent<GameCoins>();
                    string value = chatField.text.Substring(5).Trim();
                    double money = double.Parse(value);
                    Debug.Log("Admin code");
                    gameCoins.CheatCoins(-money);
                }
                else
                {
                    Debug.Log("không tìm thấy game object coin");
                }
            }
            else
            {
                Debug.Log("không tìm thấy API");
            }
        }
        else
        {
            Debug.Log("không tim thấy cửa hàng 711");
        }
    }


    public void DebugReturn(DebugLevel level, string message)
    {
        Debug.Log("Photon Chat Debug - Level: " + level + " Message: " + message);
    }

    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("Chat State Changed: " + state);
    }

    public void OnConnected()
    {
        Debug.Log("Connect to chat");
        isConnected = true;
        chatClient.Subscribe(new string[] { "FPT Channel" });
    }

    public void OnDisconnected()
    {
        throw new System.NotImplementedException();
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        /*for (int i = 0; i < senders.Length; i++)
        {
            if (pannelChat != null && chatPrefab != null)
            {
                GameObject newChat = Instantiate(chatPrefab, pannelChat.transform);
                newChat.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = username;
                newChat.transform.GetChild(2).GetComponent<TMP_Text>().text = chatField.text;

                if (username != null)
                {

                    newChat.transform.GetChild(1).GetChild(0).GetChild(0).GetComponent<TMP_Text>().text = senders[i];
                }
                else
                {
                    Debug.Log("Không thấy text tên");
                }

                if (chatField.text != null)
                {
                    newChat.transform.GetChild(2).GetComponent<TMP_Text>().text = messages[i].ToString();
                }
                else
                {
                    Debug.Log("Không thấy text chat");
                }
            }
        }*/

        string mgs = "";
        for (int i = 0; i < senders.Length; i++)
        {
            mgs = string.Format("{0}: {1}", senders[i], messages[i]);

            chatDisplay.text += "\n" + mgs;

            Debug.Log(mgs);
        }
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
        Debug.Log("Subcribed on " + channels);
    }

    public void OnUnsubscribed(string[] channels)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserSubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        throw new System.NotImplementedException();
    }
}
