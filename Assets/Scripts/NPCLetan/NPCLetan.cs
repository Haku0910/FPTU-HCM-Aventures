using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCLetan : MonoBehaviour
{
    public GameObject prefabWithToActiveReference; // K�o v� th? Prefab v�o tr??ng n�y trong Inspector
    public GameObject toActive; // K�o v� th? GameObject "ToActive" v�o tr??ng n�y trong Inspector

    private void Start()
    {
        UIMessageButton messageButton = prefabWithToActiveReference.GetComponent<UIMessageButton>();
        if (messageButton != null)
        {
            messageButton.SetToActiveReference(toActive);
        }
    }
}
