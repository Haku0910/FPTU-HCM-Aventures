using Photon.Pun;
using TMPro;

public class NameUI : MonoBehaviourPunCallbacks
{
    public TMP_Text nameText;
    private PhotonView photonView;

    // Start is called before the first frame update
    private void Start()
    {
        photonView = GetComponent<PhotonView>();
        UpdatePlayerUI();
    }
    private void UpdatePlayerUI()
    {
        if (photonView.IsMine)
        {

            nameText.text = PhotonNetwork.LocalPlayer.NickName;
        }
        else
        {
            nameText.text = photonView.Owner.NickName;

        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
