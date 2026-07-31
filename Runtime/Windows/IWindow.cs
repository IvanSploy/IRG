namespace IRG.Windows
{
    public interface IWindow
    {
        string Name { get; }
        int Level { get; }
        bool IsOpen { get; }
        void Open();
        void Close();
    }
}