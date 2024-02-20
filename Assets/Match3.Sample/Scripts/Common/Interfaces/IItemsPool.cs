namespace Match3
{
    public interface IItemsPool<TItem>
    {
        TItem GetItem();
        void ReturnItem(TItem item);
    }
}