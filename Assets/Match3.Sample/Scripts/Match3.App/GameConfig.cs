


namespace Match3
{
    public class GameConfig<TGridSlot> where TGridSlot : IGridSlot
    {
        public IGameBoard<TGridSlot> GameBoard { get; set; }
        public IItemSwapper<TGridSlot> ItemSwapper { get; set; }
        public IGameBoardSolver<TGridSlot> GameBoardSolver { get; set; }
        public ILevelGoalsProvider<TGridSlot> LevelGoalsProvider { get; set; }
        public ISolvedSequencesConsumer<TGridSlot>[] SolvedSequencesConsumers { get; set; }
    }
}