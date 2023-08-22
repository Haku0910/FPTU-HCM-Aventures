using Photon.Pun;
using TMPro;
using UnityEngine;

public class InteractionPromptUI : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject uiPannel;
    [SerializeField] private TextMeshProUGUI promptText;

    private void Start()
    {
      
        uiPannel.SetActive(false);
    }

    public bool IsDisplayText = false;

    public void SetUp(string text)
    {
      
        if (!photonView.IsMine)
        {
            return;
        }
        promptText.text = text;
        uiPannel.SetActive(true);
        IsDisplayText = true;
    }

    public void Close()
    {
        uiPannel.SetActive(false);
        IsDisplayText = false;
    }
}
