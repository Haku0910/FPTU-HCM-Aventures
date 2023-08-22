using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MakeItButton : MonoBehaviour
{
    public GameObject toActive;
    private Camera characterCamera;

    private void Start()
    {
        characterCamera = GameObject.Find("CameraTarget").GetComponent<Camera>();

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Ki?m tra ng??i ch?i nh?p chu?t tr�i
        {
            Ray ray = characterCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) // Th?c hi?n Raycast t? v? tr� chu?t
            {
                if (hit.collider.gameObject == gameObject) // Ki?m tra xem Raycast ?� ch?m v�o n�t button hay kh�ng
                {
                    // X? l� s? ki?n khi ng??i ch?i nh?n v�o n�t button
                    Debug.Log("Button clicked!");
                    toActive.SetActive(true);
                }
            }
        }
    }
    
}
