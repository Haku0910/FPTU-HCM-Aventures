using Photon.Pun;
using UnityEngine;

public class PlayerSetup : MonoBehaviourPunCallbacks
{
    public Camera FPSCamera;
    private CharacterMoving characterMoving;

    // Start is called before the first frame update
    void Start()
    {
        characterMoving = GetComponent<CharacterMoving>();
        if (photonView.IsMine)
        {
            FPSCamera.enabled = true;
            
        }
        else
        {
            characterMoving.enabled = false;
            FPSCamera.enabled = false;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}