using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private bool isInteracting = false; // Bi?n c? ?? ch? k�ch ho?t t??ng t�c m?t l?n

    private void Update()
    {
        IInteractable interactable = GetInteractableObject(); // L?y ??i t??ng c� th? t??ng t�c g?n nh?t
        if (interactable != null)
        {
            float distance = Vector3.Distance(transform.position, interactable.GetTransform().position);
            float minimumDistance = 2f; // Kho?ng c�ch t?i thi?u ?? coi l� "g?n"

            if (distance <= minimumDistance && !isInteracting) // Ki?m tra c? kho?ng c�ch v� isInteracting
            {
                interactable.Interact(transform);
                isInteracting = true; // ?�nh d?u ?� k�ch ho?t t??ng t�c
            }
            else if (distance > minimumDistance && isInteracting) // Ki?m tra khi ng??i ch?i ?i xa ??i t??ng
            {
                isInteracting = false; // ?�nh d?u kh�ng c�n k�ch ho?t t??ng t�c
            }
        }
    }


    public IInteractable GetInteractableObject()
    {
        List<IInteractable> interactableList = new List<IInteractable>(); // T?o danh s�ch interactableList ?? ch?a c�c ??i t??ng c� th? t??ng t�c
        float interactRange = 3f; // Kho?ng c�ch t??ng t�c

        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange); // L?y danh s�ch collider n?m trong ph?m vi t??ng t�c

        foreach (Collider collider in colliderArray) // Duy?t qua t?ng collider trong danh s�ch
        {
            if (collider.TryGetComponent(out IInteractable interactable)) // Ki?m tra xem collider c� th�nh ph?n IInteractable kh�ng
            {
                interactableList.Add(interactable); // N?u c�, th�m v�o danh s�ch interactableList
            }
        }

        IInteractable closestInteractable = null; // Kh?i t?o ??i t??ng closestInteractable ?? l?u tr? ??i t??ng c� th? t??ng t�c g?n nh?t

        foreach (IInteractable interactable in interactableList) // Duy?t qua danh s�ch interactableList
        {
            if (closestInteractable == null) // N?u ch?a c� ??i t??ng g?n nh?t
            {
                closestInteractable = interactable; // G�n ??i t??ng hi?n t?i l� ??i t??ng g?n nh?t
            }
            else
            {
                // So s�nh kho?ng c�ch gi?a PlayerInteract v� ??i t??ng hi?n t?i v?i ??i t??ng g?n nh?t
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) <
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position))
                {
                    // N?u ??i t??ng hi?n t?i g?n h?n, g�n ??i t??ng hi?n t?i l� ??i t??ng g?n nh?t
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable; // Tr? v? ??i t??ng c� th? t??ng t�c g?n nh?t
    }
}
