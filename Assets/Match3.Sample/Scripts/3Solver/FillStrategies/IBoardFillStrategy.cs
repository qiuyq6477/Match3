using System.Collections.Generic;


namespace Match3
{
    public interface IBoardFillStrategy<TGridSlot> where TGridSlot : IGridSlot
    {
        string Name { get; }

        IEnumerable<IJob> GetFillJobs(IGameBoard<TGridSlot> gameBoard);
        IEnumerable<IJob> GetSolveJobs(IGameBoard<TGridSlot> gameBoard, SolvedData<TGridSlot> solvedData);
    }
}