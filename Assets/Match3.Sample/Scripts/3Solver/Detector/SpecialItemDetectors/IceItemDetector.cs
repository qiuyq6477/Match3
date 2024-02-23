using System.Collections.Generic;




namespace Match3
{
    public class IceItemDetector : ISpecialItemDetector<IGridSlot>
    {
        public IEnumerable<IGridSlot> GetSpecialItemGridSlots(IGameBoard<IGridSlot> gameBoard,
            IGridSlot gridSlot)
        {
            if (gridSlot.State.TypeId == (int) TileType.Ice)
            {
                yield return gridSlot;
            }
        }
    }
}