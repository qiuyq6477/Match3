using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Match3
{
    public abstract class BaseFillStrategy : IBoardFillStrategy<IUnityGridSlot>
    {
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly UnityGameBoardRenderer _gameBoardRenderer;

        protected BaseFillStrategy(AppContext appContext)
        {
            _itemsPool = appContext.Resolve<IItemsPool<IUnityItem>>();
            _gameBoardRenderer = appContext.Resolve<UnityGameBoardRenderer>();
        }

        public abstract string Name { get; }

        public virtual IEnumerable<IJob> GetFillJobs(IGameBoard<IUnityGridSlot> gameBoard)
        {
            var itemsToShow = new List<IUnityItem>();

            for (var rowIndex = 0; rowIndex < gameBoard.RowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < gameBoard.ColumnCount; columnIndex++)
                {
                    var gridSlot = gameBoard[rowIndex, columnIndex];
                    if (gridSlot.CanSetItem == false)
                    {
                        continue;
                    }

                    var item = GetItemFromPool();
                    item.SetWorldPosition(GetWorldPosition(gridSlot.GridPosition));

                    gridSlot.SetItem(item);
                    itemsToShow.Add(item);
                }
            }

            return new[] { new ItemsShowJob(itemsToShow) };
        }

        public abstract IEnumerable<IJob> GetSolveJobs(IGameBoard<IUnityGridSlot> gameBoard,
            SolvedData<IUnityGridSlot> solvedData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return _gameBoardRenderer.GetWorldPosition(gridPosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IUnityItem GetItemFromPool()
        {
            return _itemsPool.GetItem();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ReturnItemToPool(IUnityItem item)
        {
            _itemsPool.ReturnItem(item);
        }
    }
}