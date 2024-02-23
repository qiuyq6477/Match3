


namespace Match3
{
    public interface IGameBoardSolver<TGridSlot> where TGridSlot : IGridSlot
    {
        SolvedData<TGridSlot> Solve(IGameBoard<TGridSlot> gameBoard, params GridPosition[] gridPositions);
    }
}