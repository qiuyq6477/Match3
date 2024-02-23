namespace Match3
{
    public interface IGridSlotState
    {
        int TypeId { get; }
        bool IsLocked { get; }
        bool CanContainItem { get; }
    }
}