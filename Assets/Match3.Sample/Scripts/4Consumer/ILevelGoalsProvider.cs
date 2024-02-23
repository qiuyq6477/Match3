

namespace Match3
{
    public interface ILevelGoalsProvider<TGridSlot> where TGridSlot : IGridSlot
    {
        LevelGoal<TGridSlot>[] GetLevelGoals(int level, IGameBoard<TGridSlot> gameBoard);
    }
}