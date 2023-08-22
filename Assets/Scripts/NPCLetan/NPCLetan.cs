using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLetan : MonoBehaviour
{
    public GameObject prefabWithToActiveReference; // Kéo và th? Prefab vào tr??ng này trong Inspector
    public GameObject toActive; // Kéo và th? GameObject "ToActive" vào tr??ng này trong Inspector

    private void Start()
    {
        UIMessageButton messageButton = prefabWithToActiveReference.GetComponent<UIMessageButton>();
        if (messageButton != null)
        {
            messageButton.SetToActiveReference(toActive);
        }
    }
}
