



using UnityEngine;

namespace Match3
{
    public class CollectRowMaxItems : LevelGoal<IGridSlot>
    {
        private readonly int _maxRowLength;

        public CollectRowMaxItems(IGameBoard<IGridSlot> gameBoard)
        {
            _maxRowLength = GetMaxRowLength(gameBoard);
        }

        public override void OnSequencesSolved(SolvedData<IGridSlot> solvedData)
        {
            foreach (var sequence in solvedData.SolvedSequences)
            {
                if (sequence.SequenceDetectorType != typeof(HorizontalLineDetector<IGridSlot>))
                {
                    continue;
                }

                if (sequence.SolvedGridSlots.Count == _maxRowLength)
                {
                    MarkAchieved();
                }
            }
        }

        private int GetMaxRowLength(IGameBoard<IGridSlot> gameBoard)
        {
            var maxRowLength = 0;

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                var maxRowSlots = 0;
                var availableSlots = 0;

                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    if (gameBoard[rowIndex, columnIndex].State.CanContainItem)
                    {
                        availableSlots++;
                        continue;
                    }

                    if (availableSlots > maxRowSlots)
                    {
                        maxRowSlots = availableSlots;
                    }

                    availableSlots = 0;
                }

                var maxLength = Mathf.Max(maxRowSlots, availableSlots);
                if (maxLength > maxRowLength)
                {
                    maxRowLength = maxLength;
                }
            }

            return maxRowLength;
        }
    }
}