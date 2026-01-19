namespace IRG
{
    public class Signal
    {
        public string Name;
        public object Data;

        public Signal(string name, object data = null)
        {
            Name = name;
            Data = data;
        }
    }
}