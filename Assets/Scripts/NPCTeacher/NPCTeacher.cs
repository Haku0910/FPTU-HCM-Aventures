using UnityEngine;

public class NPCTeacher : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject toActivate;
    [SerializeField] private Transform standingPoint;
    [SerializeField] private string prompt;



    public string InteractionPromp => prompt;

    public void CloseButton()
    {
        toActivate.SetActive(false);
    }


    public bool Interact(Interactor interactor)
    {
        toActivate.SetActive(true);
        return true;
    }

    /*private void OnTriggerEnter(Collider other)
    {
*//*        PhotonView photonView = GetComponent<PhotonView>();
*//*    
    
        toActivate.SetActive(true);
    }*/

}
