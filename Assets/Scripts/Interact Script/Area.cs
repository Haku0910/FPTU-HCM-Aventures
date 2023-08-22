using UnityEngine;

public class Area : MonoBehaviour, Interactable
{
    private string prompt;

    public string InteractionPromp => prompt;

    public bool Interact(Interactor interactor)
    {
        Debug.Log("Contact with object: " + prompt);
        return true;
    }

    private void Start()
    {
        prompt = gameObject.name;
    }

}
