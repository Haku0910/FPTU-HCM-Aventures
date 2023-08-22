using Photon.Pun;
using UnityEngine;

public class ButtonManager : MultiplayerSingleton<ButtonManager>
{
    private PhotonView photonView;


    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        if (photonView.IsMine)
        {
            this.gameObject.SetActive(true);

        }
        else
        {
            this.gameObject.SetActive(false);
        }

    }

    public void HideButton()
    {
        this.gameObject.SetActive(false);


    }
    public void ShowButton()
    {
        this.gameObject.SetActive(true);

    }

}
