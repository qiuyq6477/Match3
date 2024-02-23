using System.Collections.Generic;
using Match3;
using Match3;

namespace Match3
{
    public static class ItemsPoolExtensions
    {
        public static void ReturnAllItems(this IItemsPool<IItem> itemsPool, IEnumerable<IGridSlot> gridSlots)
        {
            foreach (var gridSlot in gridSlots)
            {
                if (gridSlot.Item == null)
                {
                    continue;
                }

                itemsPool.ReturnItem(gridSlot.Item);
                gridSlot.Item.Hide();
                gridSlot.Clear();
            }
        }
    }
}