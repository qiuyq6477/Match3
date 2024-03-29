
namespace Match3
{
    public interface ISequenceDetector<TGridSlot> where TGridSlot : IGridSlot
    {
        ItemSequence<TGridSlot> GetSequence(IGameBoard<TGridSlot> gameBoard, GridPosition gridPosition);
    }
}