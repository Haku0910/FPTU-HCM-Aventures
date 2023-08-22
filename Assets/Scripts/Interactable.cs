public interface Interactable
{
    public string InteractionPromp { get; }

    public bool Interact(Interactor interactor);
}
