using System.Collections.Generic;

namespace Match3
{
    public class StoneItemDetector : ISpecialItemDetector<IGridSlot>
    {
        private readonly GridPosition[] _lookupDirections;

        public StoneItemDetector()
        {
            _lookupDirections = new[]
            {
                GridPosition.Up,
                GridPosition.Down,
                GridPosition.Left,
                GridPosition.Right
            };
        }

        public IEnumerable<IGridSlot> GetSpecialItemGridSlots(IGameBoard<IGridSlot> gameBoard,
            IGridSlot gridSlot)
        {
            if (gridSlot.IsMovable == false)
            {
                yield break;
            }

            foreach (var lookupDirection in _lookupDirections)
            {
                var lookupPosition = gridSlot.GridPosition + lookupDirection;
                if (gameBoard.IsPositionOnGrid(lookupPosition) == false)
                {
                    continue;
                }

                var lookupGridSlot = gameBoard[lookupPosition];
                if (lookupGridSlot.State.GroupId == (int) TileGroup.Stone)
                {
                    yield return lookupGridSlot;
                }
            }
        }
    }
}