
namespace Match3
{
    public interface IGridSlot
    {
        int ItemId { get; }

        bool HasItem { get; }
        bool IsMovable { get; }
        bool CanContainItem { get; }

        IGridSlotState State { get; }
        GridPosition GridPosition { get; }
        
        bool CanSetItem { get; }
        bool NotAvailable { get; }

        IItem Item { get; }

        void SetItem(IItem item);
        void SetState(IGridSlotState state);
        void Clear();
    }
}