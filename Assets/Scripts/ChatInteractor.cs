using Photon.Pun;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChatInteractor : MonoBehaviourPunCallbacks, IPointerClickHandler
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius;
    [SerializeField] private LayerMask interactableMask;
    [SerializeField] private ChatPromptUI chatPromptUI;

    private readonly Collider[] colliders = new Collider[3];
    [SerializeField] private int numFound;
    [SerializeField] private Image targetImage;


    private ChatInteractable chatInteractable;

    private void Update()
    {
        numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, colliders, interactableMask);

        if (numFound > 0)
        {
            chatInteractable = colliders[0].GetComponent<ChatInteractable>();
            if (photonView.IsMine)
            {
                if (chatInteractable != null)
                {
                    if (!chatPromptUI.IsDisplayText)
                    {
                        chatPromptUI.SetUp(chatInteractable.ChatInteractionPromp);
                    }
                    if (Keyboard.current.eKey.wasPressedThisFrame)
                    {
                        chatInteractable.ChatInteract(this);
                    }
                }
            }
        }
        else
        {
            if (chatInteractable != null)
            {
                chatInteractable = null;
            }
            if (chatPromptUI.IsDisplayText)
            {
                chatPromptUI.Close();
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
        if (chatInteractable != null && photonView.IsMine)
        {
            Debug.Log("contact");
            chatInteractable.ChatInteract(this);
        }

    }
}
