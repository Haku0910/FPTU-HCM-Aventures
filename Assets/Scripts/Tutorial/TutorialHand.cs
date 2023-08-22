using Photon.Pun;
using UnityEngine;

public class TutorialHand : MonoBehaviour
{

    public GameObject tutorialGame;


    public void OpenTutorial()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable() { { "CheckButton", "off" } });
        tutorialGame.SetActive(true);
    }
    public void CloseTutorial()
    {
        tutorialGame.SetActive(false);

    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
