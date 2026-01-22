namespace IRG.Interaction
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        void Interact();
    }
}