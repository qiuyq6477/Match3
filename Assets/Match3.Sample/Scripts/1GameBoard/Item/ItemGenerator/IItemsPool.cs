namespace Match3
{
    public interface IItemsPool<TItem>
    {
        void Init(int capacity);
        TItem GetItem();
        void ReturnItem(TItem item);
        void Dispose();
    }
}