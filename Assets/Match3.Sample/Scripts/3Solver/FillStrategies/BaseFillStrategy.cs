using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = System.Random;

namespace Match3
{
    public abstract class BaseFillStrategy : IBoardFillStrategy<IGridSlot>
    {
        private readonly Random _random;
        private readonly IItemsPool<IItem> _itemsPool;
        private readonly UnityGameBoardRenderer _gameBoardRenderer;
        private readonly BaseGame<IGridSlot> _baseGame;
        protected BaseFillStrategy(AppContext appContext)
        {
            _random = new Random();
            _itemsPool = appContext.Resolve<IItemsPool<IItem>>();
            _gameBoardRenderer = appContext.Resolve<UnityGameBoardRenderer>();
            _baseGame = appContext.Resolve<UnityGame>();
        }

        public abstract string Name { get; }

        public virtual IEnumerable<IJob> GetFillJobs(IGameBoard<IGridSlot> gameBoard)
        {
            var itemsToShow = new List<IItem>();

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

        public abstract IEnumerable<IJob> GetSolveJobs(IGameBoard<IGridSlot> gameBoard,
            SolvedData<IGridSlot> solvedData);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return _baseGame.GameBoard.GetWorldPosition(gridPosition);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected IItem GetItemFromPool()
        {
            var item = _itemsPool.GetItem();
            var sprites = _baseGame.GetSprite();
            var index = _random.Next(0, sprites.Length);
            item.SetSprite(index, sprites[index]);
            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void ReturnItemToPool(IItem item)
        {
            _itemsPool.ReturnItem(item);
        }
    }
}