using Match3;

namespace Match3
{
    public interface ISolvedSequencesConsumer<TGridSlot> where TGridSlot : IGridSlot
    {
        void OnSequencesSolved(SolvedData<TGridSlot> solvedData);
    }
}