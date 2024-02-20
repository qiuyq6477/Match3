using System.Collections.Generic;
using Match3;
using Match3;
using Match3;

namespace Match3
{
    public class IceItemDetector : ISpecialItemDetector<IUnityGridSlot>
    {
        public IEnumerable<IUnityGridSlot> GetSpecialItemGridSlots(IGameBoard<IUnityGridSlot> gameBoard,
            IUnityGridSlot gridSlot)
        {
            if (gridSlot.State.GroupId == (int) TileGroup.Ice)
            {
                yield return gridSlot;
            }
        }
    }
}