
namespace Match3
{
    public class HorizontalLineDetector<TGridSlot> : LineDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        private readonly GridPosition[] _lineDirections;

        public HorizontalLineDetector()
        {
            _lineDirections = new[] { GridPosition.Left, GridPosition.Right };
        }

        public override ItemSequence<TGridSlot> GetSequence(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition)
        {
            return GetSequenceByDirection(gameBoard, gridPosition, _lineDirections);
        }
    }
}