using System.Collections.Generic;
using Match3;
using Match3;
using Match3;

namespace Match3
{
    public class IceItemDetector : ISpecialItemDetector<IGridSlot>
    {
        public IEnumerable<IGridSlot> GetSpecialItemGridSlots(IGameBoard<IGridSlot> gameBoard,
            IGridSlot gridSlot)
        {
            if (gridSlot.State.GroupId == (int) TileGroup.Ice)
            {
                yield return gridSlot;
            }
        }
    }
}