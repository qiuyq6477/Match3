﻿using System.Collections.Generic;








namespace Match3
{
    public class FallDownFillStrategy : BaseFillStrategy
    {
        public FallDownFillStrategy(AppContext appContext) : base(appContext)
        {
        }

        public override string Name => "Fall Down Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IGridSlot> gameBoard,
            SolvedData<IGridSlot> solvedData)
        {
            var jobs = new List<IJob>();
            var itemsToHide = new List<IItem>();
            var solvedGridSlots = new HashSet<IGridSlot>();

            foreach (var solvedGridSlot in solvedData.GetSolvedGridSlots())
            {
                if (solvedGridSlot.IsMovable == false)
                {
                    continue;
                }

                if (solvedGridSlots.Add(solvedGridSlot) == false)
                {
                    continue;
                }

                var currentItem = solvedGridSlot.Item;
                itemsToHide.Add(currentItem);
                solvedGridSlot.Clear();

                ReturnItemToPool(currentItem);
            }

            foreach (var specialItemGridSlot in solvedData.GetSpecialItemGridSlots())
            {
                solvedGridSlots.Add(specialItemGridSlot);
            }

            foreach (var solvedGridSlot in solvedGridSlots)
            {
                var itemsMoveData = GetItemsMoveData(gameBoard, solvedGridSlot.GridPosition.ColumnIndex);
                if (itemsMoveData.Count != 0)
                {
                    jobs.Add(new ItemsMoveJob(itemsMoveData));
                }
            }

            solvedGridSlots.Clear();
            jobs.Add(new ItemsHideJob(itemsToHide));
            jobs.AddRange(GetFillJobs(gameBoard, 1));

            return jobs;
        }

        private List<ItemMoveData> GetItemsMoveData(IGameBoard<IGridSlot> gameBoard, int columnIndex)
        {
            var itemsDropData = new List<ItemMoveData>();

            for (var rowIndex = gameBoard.RowCount - 1; rowIndex >= 0; rowIndex--)
            {
                var gridSlot = gameBoard[rowIndex, columnIndex];
                if (gridSlot.IsMovable == false)
                {
                    continue;
                }

                if (CanDropDown(gameBoard, gridSlot, out var destinationGridPosition) == false)
                {
                    continue;
                }

                var item = gridSlot.Item;
                gridSlot.Clear();
                itemsDropData.Add(
                    new ItemMoveData(item, new[] { GetWorldPosition(destinationGridPosition) }));
                gameBoard[destinationGridPosition].SetItem(item);
            }

            itemsDropData.Reverse();
            return itemsDropData;
        }

        private IEnumerable<IJob> GetFillJobs(IGameBoard<IGridSlot> gameBoard, int delayMultiplier)
        {
            var jobs = new List<IJob>();

            for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
            {
                var itemsDropData = new List<ItemMoveData>();

                for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    if (gridSlot.CanSetItem == false)
                    {
                        continue;
                    }

                    var item = GetItemFromPool();
                    var itemGeneratorPosition = GetItemGeneratorPosition(gameBoard, rowIndex, columnIndex);
                    item.SetWorldPosition(GetWorldPosition(itemGeneratorPosition));

                    var itemDropData =
                        new ItemMoveData(item, new[] { GetWorldPosition(gridSlot.GridPosition) });

                    gridSlot.SetItem(item);
                    itemsDropData.Add(itemDropData);
                }

                if (itemsDropData.Count > 0)
                {
                    jobs.Add(new ItemsFallJob(itemsDropData, delayMultiplier));
                }
            }

            return jobs;
        }

        private GridPosition GetItemGeneratorPosition(IGameBoard<IGridSlot> gameBoard, int rowIndex, int columnIndex)
        {
            while (rowIndex >= 0)
            {
                var gridSlot = gameBoard[rowIndex, columnIndex];
                if (gridSlot.NotAvailable)
                {
                    return new GridPosition(rowIndex, columnIndex);
                }

                rowIndex--;
            }

            return new GridPosition(-1, columnIndex);
        }

        private bool CanDropDown(IGameBoard<IGridSlot> gameBoard, IGridSlot gridSlot,
            out GridPosition destinationGridPosition)
        {
            var destinationGridSlot = gridSlot;

            while (gameBoard.CanMoveDown(destinationGridSlot, out var bottomGridPosition))
            {
                destinationGridSlot = gameBoard[bottomGridPosition];
            }

            destinationGridPosition = destinationGridSlot.GridPosition;
            return destinationGridSlot != gridSlot;
        }
    }
}