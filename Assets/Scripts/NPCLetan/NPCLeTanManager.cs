using Photon.Pun;
using UnityEngine;

public class NPCLeTanManager : MonoBehaviourPunCallbacks, Interactable
{
    [SerializeField] private GameObject toActivate;

    [SerializeField] private Transform standingPoint;
    [SerializeField] private string prompt;



    public string InteractionPromp => prompt;


    /*    private async void OnTriggerEnter(Collider other)
        {
            PhotonView photonView = other.GetComponent<PhotonView>();
            if (!photonView.IsMine)
            {
                return;
            }

            avatar = other.transform;

            // disable player input
            *//*            avatar.GetComponent<PlayerInput>().enabled = false;
            *//*
            await Task.Delay(50);

            // teleport the avatar to standing point
            avatar.position = standingPoint.position;
            avatar.rotation = standingPoint.rotation;

            // disable main cam, enable dialog cam
            mainCamera.SetActive(false);
            toActivate.SetActive(true);
            iconBag.SetActive(false);
            iconMission.SetActive(false);
            // d?splay cursor
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;

        }*/

    public void Recover()
    {
        /*        avatar.GetComponent<PlayerInput>().enabled = true;*/
        toActivate.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

   

    public bool Interact(Interactor interactor)
    {
        toActivate.SetActive(true);
        return true;
    }
}
