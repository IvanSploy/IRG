namespace IRG.Graphs
{
    public interface IConvertible<T>
    {
        T ToData();
        void FromData(T data);
    }
}