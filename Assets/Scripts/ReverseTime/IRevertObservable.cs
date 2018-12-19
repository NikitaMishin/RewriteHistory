namespace ReverseTime
{
    public interface IRevertObservable
    {
        void AddListener(IRevertListener obj);
        void RemoveListener(IRevertListener obj);
    
    }
}