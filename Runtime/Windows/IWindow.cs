namespace IRG.Windows
{
    public interface IWindow
    {
        string Name { get; }
        bool IsOpen { get; }
        void Open();
        void Close();
    }
}