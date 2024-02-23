




namespace Match3
{
    public class LevelGoalsProvider : ILevelGoalsProvider<IGridSlot>
    {
        public LevelGoal<IGridSlot>[] GetLevelGoals(int level, IGameBoard<IGridSlot> gameBoard)
        {
            return new LevelGoal<IGridSlot>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}