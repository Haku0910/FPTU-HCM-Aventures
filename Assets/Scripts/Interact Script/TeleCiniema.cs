using UnityEngine;
using UnityEngine.SceneManagement;

public class TeleCiniema : MonoBehaviour, Interactable
{
    [SerializeField] private string prompt;

    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Contact Teleport from Cinema");
        SceneManager.LoadScene("Playground1");
        return true;
    }
}
