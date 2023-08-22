using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool isInteracting = false; // Bi?n c? ?? ch? kích ho?t t??ng tác m?t l?n

    private void Update()
    {
        IInteractable interactable = GetInteractableObject(); // L?y ??i t??ng có th? t??ng tác g?n nh?t
        if (interactable != null)
        {
            float distance = Vector3.Distance(transform.position, interactable.GetTransform().position);
            float minimumDistance = 2f; // Kho?ng cách t?i thi?u ?? coi là "g?n"

            if (distance <= minimumDistance && !isInteracting) // Ki?m tra c? kho?ng cách và isInteracting
            {
                interactable.Interact(transform);
                isInteracting = true; // ?ánh d?u ?ã kích ho?t t??ng tác
            }
            else if (distance > minimumDistance && isInteracting) // Ki?m tra khi ng??i ch?i ?i xa ??i t??ng
            {
                isInteracting = false; // ?ánh d?u không còn kích ho?t t??ng tác
            }
        }
    }


    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>(); // T?o danh sách interactableList ?? ch?a các ??i t??ng có th? t??ng tác
        float interactRange = 3f; // Kho?ng cách t??ng tác

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange); // L?y danh sách collider n?m trong ph?m vi t??ng tác

        foreach (Collider collider in colliderArray) // Duy?t qua t?ng collider trong danh sách
        {
            if (collider.TryGetComponent(out IInteractable interactable)) // Ki?m tra xem collider có thành ph?n IInteractable không
            {
                interactableList.Add(interactable); // N?u có, thêm vào danh sách interactableList
            }
        }

        IInteractable closestInteractable = null; // Kh?i t?o ??i t??ng closestInteractable ?? l?u tr? ??i t??ng có th? t??ng tác g?n nh?t

        foreach (IInteractable interactable in interactableList) // Duy?t qua danh sách interactableList
        {
            if (closestInteractable == null) // N?u ch?a có ??i t??ng g?n nh?t
            {
                closestInteractable = interactable; // Gán ??i t??ng hi?n t?i là ??i t??ng g?n nh?t
            }
            else
            {
                // So sánh kho?ng cách gi?a PlayerInteract và ??i t??ng hi?n t?i v?i ??i t??ng g?n nh?t
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    // N?u ??i t??ng hi?n t?i g?n h?n, gán ??i t??ng hi?n t?i là ??i t??ng g?n nh?t
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable; // Tr? v? ??i t??ng có th? t??ng tác g?n nh?t
    }
}
