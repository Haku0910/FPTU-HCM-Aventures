using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable
{
    private NPCHeadLookAt npcHeadLookAt;

    private void Awake()
    {
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform)
    {
        StartCoroutine(ShowDialogue(interactorTransform));


    }
    private IEnumerator ShowDialogue(Transform interactorTransform)
    {
        ChatBuble.Create(transform, new Vector3(-.3f, 1.7f, 0f), "Hello!");

        float playerHeight = 1.7f;
        npcHeadLookAt.LookAtPosition(interactorTransform.position + Vector3.up * playerHeight);

        yield return new WaitForSeconds(3f); // ??i 3 giây tr??c khi hi?n th? câu ti?p theo

        ChatBuble.Create(transform, new Vector3(-.3f, 1.7f, 0f), "How are you?");

        // Các hành ??ng khác sau khi hi?n th? câu h?i tho?i ti?p theo
    }

    /*public string GetInteractText()
    {
        return interactText;
    }*/

    public Transform GetTransform()
    {
        return transform;
    }

}