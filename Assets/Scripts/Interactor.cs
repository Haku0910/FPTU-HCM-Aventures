using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Interactor : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private InteractionPromptUI interactionPromptUI;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;
    [SerializeField] private Image targetImage;

    [SerializeField] private AudioSource audio;

    public List<LocationData> location;
    private List<LocationData> locationDataList = new List<LocationData>();


    private Interactable interactable;


    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

        if (numFound > 0)
        {
            interactable = colliders[0].GetComponent<Interactable>();

            if (photonView.IsMine)
            {
                if (interactable != null)
                {

                    if (!interactionPromptUI.IsDisplayText)
                    {
                        interactionPromptUI.SetUp(interactable.InteractionPromp);
                    }
                    if (Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        interactable.Interact(this);
                    }

                }
            }
        }
        else
        {
            if (interactable != null)
            {
                interactable = null;
            }
            if (interactionPromptUI.IsDisplayText)
            {
                interactionPromptUI.Close();
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (interactable != null && photonView.IsMine)
        {
            Debug.Log("contact");
            audio.Play();
            interactable.Interact(this);
        }
    }

}
