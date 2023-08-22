using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleport : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt;

    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Contact Teleport from School");
        SceneManager.LoadScene("Hall A");
        return true;
    }
}
