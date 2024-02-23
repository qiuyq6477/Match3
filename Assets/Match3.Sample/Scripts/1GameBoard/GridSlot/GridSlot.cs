using System;
using System.Runtime.CompilerServices;
using Match3;
using Match3;
using Match3;

namespace Match3
{
    public class GridSlot : IGridSlot
    {
        public GridSlot(IGridSlotState state, GridPosition gridPosition)
        {
            State = state;
            GridPosition = gridPosition;
        }

        public int ItemId => Item.ContentId;

        public bool HasItem => Item != null;
        public bool IsMovable => State.IsLocked == false && HasItem;
        public bool CanContainItem => State.CanContainItem;
        public bool CanSetItem => State.CanContainItem && HasItem == false;
        public bool NotAvailable => State.CanContainItem == false || State.IsLocked;

        public IItem Item { get; private set; }
        public IGridSlotState State { get; private set; }
        public GridPosition GridPosition { get; }

        public void SetState(IGridSlotState state)
        {
            State = state;
        }

        public void SetItem(IItem item)
        {
            EnsureItemIsNotNull(item);

            Item = item;
        }

        public void Clear()
        {
            if (State.CanContainItem == false)
            {
                throw new InvalidOperationException("Can not clear an unavailable grid slot.");
            }

            Item = default;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void EnsureItemIsNotNull(IItem item)
        {
            if (item == null)
            {
                throw new NullReferenceException(nameof(item));
            }
        }
    }
}