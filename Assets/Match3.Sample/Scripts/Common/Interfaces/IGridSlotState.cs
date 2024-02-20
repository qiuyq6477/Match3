namespace Match3
{
    public interface IGridSlotState
    {
        int GroupId { get; }
        bool IsLocked { get; }
        bool CanContainItem { get; }
    }
}