using Google;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

public class LoginController : MonoBehaviourPunCallbacks
{
    private const string apiUrl = "https://anhkiet-001-site1.htempurl.com/api/Events/events/time/";

    private string imageUrl;
    private string webClientId = "668814474990-4p5vepvjsqil4e9cl8imk3covmv6evmo.apps.googleusercontent.com";
    public TMP_InputField userNameField;
    public TMP_InputField passWordField;
    Firebase.DependencyStatus dependencyStatus = Firebase.DependencyStatus.UnavailableOther;
    Firebase.Auth.FirebaseAuth auth;
    Firebase.Auth.FirebaseUser user;
    public string schoolId;
    private bool joinedRoom = false;

    private string schoolName;
    private GoogleSignInConfiguration configuration;
    private UserAPI userAPI;
    private PlayerAPI playerAPI;
    public GameObject SignInScreen;
    public TMP_Text errorText;
    public GameObject errorGameText;
    private bool isPlayer = true;
    private bool isCheckPlay = true;
    private SessionManager sessionManager = new SessionManager();
    public GameObject loading;
    // Defer the configuration creation until Awake so the web Client ID
    // Can be set via the property inspector in the Editor.
    void Awake()
    {
        configuration = new GoogleSignInConfiguration
        {
            WebClientId = webClientId,
            RequestIdToken = true
        };
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start()
    {
        InitFirebase();
        PhotonNetwork.AutomaticallySyncScene = true;
        // T?m d?ng h?ng ??i tin nh?n c?a Photon
        PhotonNetwork.IsMessageQueueRunning = false;
        userAPI = GetComponent<UserAPI>();
        playerAPI = GetComponent<PlayerAPI>();
    }

    void InitFirebase()
    {
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
    }
    #region LoginGmail
    /*public void GoogleSignInClick()
    {
        GoogleSignIn.Configuration = configuration;
        GoogleSignIn.Configuration.UseGameSignIn = false;
        GoogleSignIn.Configuration.RequestIdToken = true;
        GoogleSignIn.Configuration.RequestEmail = true;

        GoogleSignIn.DefaultInstance.SignIn().ContinueWith(OnAuthenticationFinished);
    }

    void OnAuthenticationFinished(Task<GoogleSignInUser> task)
    {
        if (task.IsFaulted)
        {
            Debug.LogError("Fault");
        }
        else if (task.IsCanceled)
        {
            Debug.LogError("Login Cancel");
        }
        else
        {
            Firebase.Auth.Credential credential = Firebase.Auth.GoogleAuthProvider.GetCredential(task.Result.IdToken, null);
            auth.SignInWithCredentialAsync(credential).ContinueWithOnMainThread(task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SignInWithCredentialAsync was canceled");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SignInWithCredentialAsync encountered an error: " + task.Exception);
                    return;
                }

                user = auth.CurrentUser;
                LoginScreen.SetActive(false);
                CodeScreen.SetActive(false);
                SignInScreen.SetActive(false);

                // Ng??i ch?i t?n t?i

                authenticationFinished = true;
                PhotonNetwork.LocalPlayer.NickName = user.Email;
                ConnectToPhoton();
                *//*// Ki?m tra quy?n ?i?u khi?n tr??c khi k?t n?i v?i Photon
                    if (authenticationFinished && !joinedRoom)
                    {
                        PhotonNetwork.JoinOrCreateRoom("Shared Room", new RoomOptions { MaxPlayers = 10 }, TypedLobby.Default);
                        joinedRoom = true;
                    }*//*
            });
        }
    }*/
    #endregion
    #region CheckIsUser
    public IEnumerator CheckUserByEmail(string email, string passcode)
    {
        string url = "https://anhkiet-001-site1.htempurl.com/api/Accounts/loginunity";
        LoginData loginData = new LoginData
        {
            email = email,
            passcode = passcode
        };

        // Chuyển LoginData thành dạng JSON
        string jsonData = JsonUtility.ToJson(loginData);
        using (UnityWebRequest request = UnityWebRequest.Post(url, "POST"))
        {
            byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(jsonData);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            // Thiết lập tiêu đề "Content-Type" cho yêu cầu POST
            request.SetRequestHeader("Content-Type", "application/json");

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string responseData = request.downloadHandler.text;
                Wrapper<LoginResponse> loginResponse = JsonUtility.FromJson<Wrapper<LoginResponse>>(responseData);
                // Lưu trữ token vào PlayerPrefs hoặc biến tạm thời để sử dụng sau này
                PlayerPrefs.SetString("token", loginResponse.data.token);
                Debug.Log("Login successful. Token: " + loginResponse.data.token);
                if (loginResponse != null && loginResponse.data.studentId != null)
                {
                    using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl + loginResponse.data.schoolId))
                    {
                        string authToken = PlayerPrefs.GetString("token");
                        webRequest.SetRequestHeader("Authorization", "Bearer " + authToken);
                        yield return webRequest.SendWebRequest();

                        if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                        {
                            Debug.Log(webRequest.error);
                        }
                        else
                        {
                            schoolId = loginResponse.data.schoolId;
                            PlayerPrefs.SetString("schoolId", schoolId);
                            Debug.Log("Status: " + loginResponse.data.schoolStatus);
                            if (loginResponse.data.schoolStatus.Equals("ACTIVE"))
                            {
                                DataModel eventData = JsonUtility.FromJson<DataModel>(webRequest.downloadHandler.text);
                                if (eventData.data == null || eventData.data.taskDtos == null || eventData.data.eventName == null)
                                {
                                    isCheckPlay = false;
                                    errorGameText.SetActive(true);
                                    errorText.text = "Sự kiện hiện tại không có!";
                                    Debug.Log("Không tồn tại sự kiện");
                                    userNameField.text = "";
                                    passWordField.text = "";
                                    loading.gameObject.SetActive(false);
                                }
                                else
                                {
                                    if (loginResponse.data.nickname != null && loginResponse.data.isplayer == true)
                                    {
                                        Debug.Log("Người chơi tồn tại");
                                        Debug.Log("player nick name" + loginResponse.data.nickname);
                                        PhotonNetwork.LocalPlayer.NickName = loginResponse.data.nickname;
                                        schoolName = loginResponse.data.schoolName;
                                        isPlayer = true;
                                        ConnectToPhoton();
                                        loading.gameObject.SetActive(false);

                                    }
                                    else
                                    {
                                        Debug.Log("Người chơi không tồn tại");
                                        PhotonNetwork.LocalPlayer.NickName = loginResponse.data.studentId;
                                        schoolName = loginResponse.data.schoolName;
                                        isPlayer = false;
                                        ConnectToPhoton();
                                        loading.gameObject.SetActive(false);

                                    }
                                }
                            }
                            else
                            {
                                isCheckPlay = false;
                                errorGameText.SetActive(true);
                                errorText.text = "Có vẻ tài khoản này không có trong danh sách tham gia sự kiện";
                                Debug.Log("Không tồn tại");
                                userNameField.text = "";
                                passWordField.text = "";
                                loading.gameObject.SetActive(false);

                            }
                        }
                    }
                    loading.gameObject.SetActive(false);
                }
                else
                {
                    isCheckPlay = false;
                    errorGameText.SetActive(true);
                    errorText.text = "Tài khoản hoặc mật khẩu không đúng";
                    Debug.Log("Không tồn tại");
                    userNameField.text = "";
                    passWordField.text = "";
                    loading.gameObject.SetActive(false);
                }
            }
            else
            {
                isCheckPlay = false;
                Debug.Log("Lỗi khi gửi yêu cầu đến API: " + request.error);
                loading.gameObject.SetActive(false);
            }
        }
        loading.gameObject.SetActive(false);
    }


    #endregion


    public void Logout()
    {
        if (auth != null)
        {
            auth.SignOut();
            GoogleSignIn.DefaultInstance.SignOut();

            // X?a d? li?u ng??i d?ng hi?n t?i
            user = null;

            // Th?c hi?n c?c t?c ??ng giao di?n ng??i d?ng sau khi ??ng xu?t

            // Hi?n th? m?n h?nh ??ng nh?p
            SignInScreen.SetActive(false);
        }
    }

    public void SignInSuccess()
    {
        Debug.Log("Sigin In");
        string email = userNameField.text.Trim();
        string passcode = passWordField.text.Trim();
        loading.gameObject.SetActive(true);

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(passcode))
        {
            Debug.Log("Vui lòng điền đầy đủ thông tin.");
            errorGameText.SetActive(true);
            errorText.text = "Vui lòng điền đầy đủ thông tin.";
            loading.gameObject.SetActive(false);
            return;
        }
        else
        {
            if (sessionManager.IsUserLoggedIn(email, passcode))
            {
                Debug.Log("Tài khoản đã đăng nhập từ một thiết bị khác.");
                loading.gameObject.SetActive(false);
                return;
            }
            else
            {
                Debug.Log("sigin vao photon");
                StartCoroutine(CheckUserByEmail(email, passcode));
            }
        }
    }

    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        if (auth.CurrentUser == null)
        {
            Debug.Log("success");
        }
    }

    public void OnDisconnect()
    {
        GoogleSignIn.DefaultInstance.Disconnect();
    }

    #region Photon Callbacks

    private void DisconnectFromPhoton()
    {
        PhotonNetwork.LeaveRoom();

        PhotonNetwork.Disconnect();


    }
    public override void OnConnected()
    {
        Debug.Log("Connect to Internet");
    }

    private void ConnectToPhoton()
    {
        if (PhotonNetwork.IsConnected)
        {
            JoinRoom();
        }
        else
        {
            PhotonNetwork.GameVersion = "1.0";
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    private void JoinRoom()
    {
        if (joinedRoom)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 10;

        PhotonNetwork.JoinOrCreateRoom(schoolName, roomOptions, TypedLobby.Default);
        joinedRoom = true;
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " is connected to Photon");


        JoinRoom();

    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName + " joined to " + PhotonNetwork.CurrentRoom.Name);

        if (PhotonNetwork.LocalPlayer.IsMasterClient)
        {
            Debug.Log("Vao");
            if (isPlayer)
            {
                PhotonNetwork.LoadLevel("Playground1");

            }
            else
            {
                PhotonNetwork.LoadLevel("TutorialNPCSence");

            }
        }
        else
        {
            Debug.Log("Ra");
        }
    }
    public void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {

        Debug.Log("Người chơi đã tham gia: " + newPlayer.NickName);
        // Cập nhật danh sách người chơi hiện có trong game
    }

    public void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Người chơi đã rời phòng: " + otherPlayer.NickName);
        // Cập nhật danh sách người chơi hiện có trong game
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Disconnected from Photon Server: " + cause);
    }

    #endregion

}
