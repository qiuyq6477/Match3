using Match3;
using Match3;
using Match3;
using Match3;

namespace Match3
{
    public class LevelGoalsProvider : ILevelGoalsProvider<IUnityGridSlot>
    {
        public LevelGoal<IUnityGridSlot>[] GetLevelGoals(int level, IGameBoard<IUnityGridSlot> gameBoard)
        {
            return new LevelGoal<IUnityGridSlot>[] { new CollectRowMaxItems(gameBoard) };
        }
    }
}