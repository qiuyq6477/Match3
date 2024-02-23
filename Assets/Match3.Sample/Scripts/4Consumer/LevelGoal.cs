using System;



namespace Match3
{
    public abstract class LevelGoal<TGridSlot> : ISolvedSequencesConsumer<TGridSlot> where TGridSlot : IGridSlot
    {
        public bool IsAchieved { get; private set; }

        public event EventHandler Achieved;

        public abstract void OnSequencesSolved(SolvedData<TGridSlot> solvedData);

        protected void MarkAchieved()
        {
            IsAchieved = true;
            Achieved?.Invoke(this, EventArgs.Empty);
        }
    }
}