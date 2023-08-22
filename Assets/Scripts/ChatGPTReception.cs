using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ChatGPTReception : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private TMP_Text scroll;
    [SerializeField] private NPCReception npcReception;
    [SerializeField] int lettersPerSecond;
    [SerializeField] List<string> lines;
    private int currentLineIndex = 0;

    string msg = "";
    string playerName = PhotonNetwork.LocalPlayer.NickName;

    private readonly string baseUrl = "https://anhkiet-001-site1.htempurl.com"; // Thay th? b?ng URL c?a API c?a b?n

    private void Start()
    {
        StartCoroutine(TypeDialog(lines[currentLineIndex]));
    }
    // Start is called before the first frame update
    public void OnSubmitButtonClicked()
    {
        string question = inputField.text;

        msg = string.Format("{0} : {1}", playerName, question);
        scroll.text += "\n" + msg;

        inputField.text = "";

        StartCoroutine(CallOpenApi(question));
    }
    public IEnumerator CallOpenApi(string question)
    {
        yield return StartCoroutine(GetOpenApiResult(question));
    }
    private IEnumerator GetOpenApiResult(string question)
    {
        string url = baseUrl + "/api/OpenApis/UseChatGpt?question=" + UnityWebRequest.EscapeURL(question);
        UnityWebRequest www = UnityWebRequest.Get(url);
        string authToken = PlayerPrefs.GetString("token");
        www.SetRequestHeader("Authorization", "Bearer " + authToken);
        www.SetRequestHeader("Content-Type", "application/json");

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            string response = www.downloadHandler.text;
            StartCoroutine(TypeDialog(response));
        }
        else
        {
            Debug.Log("Error: " + www.error);
        }
    }
    public IEnumerator TypeDialog(string line)
    {
        msg = string.Format("Lễ Tân: {0}", line);

        scroll.text += "\n" + msg;

        yield return new WaitForSeconds(1f);

        currentLineIndex++;

        if (currentLineIndex < lines.Count)
        {
            StartCoroutine(TypeDialog(lines[currentLineIndex]));
        }
        else
        {
            Debug.Log("Het");
        }
    }



}

