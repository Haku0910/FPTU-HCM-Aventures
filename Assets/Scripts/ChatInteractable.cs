public interface ChatInteractable
{
    public string ChatInteractionPromp { get; }

    public bool ChatInteract(ChatInteractor chatInteractor);
}
