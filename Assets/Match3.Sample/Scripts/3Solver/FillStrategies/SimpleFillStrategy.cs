using System.Collections.Generic;
using Match3;
using Match3;
using Match3;
using Match3;
using Match3;

namespace Match3
{
    public class SimpleFillStrategy : BaseFillStrategy
    {
        public SimpleFillStrategy(AppContext appContext) : base(appContext)
        {
        }

        public override string Name => "Simple Fill Strategy";

        public override IEnumerable<IJob> GetSolveJobs(IGameBoard<IGridSlot> gameBoard,
            SolvedData<IGridSlot> solvedData)
        {
            var itemsToHide = new List<IItem>();
            var itemsToShow = new List<IItem>();

            foreach (var solvedGridSlot in solvedData.GetUniqueSolvedGridSlots(true))
            {
                var newItem = GetItemFromPool();
                var currentItem = solvedGridSlot.Item;

                newItem.SetWorldPosition(currentItem.GetWorldPosition());
                solvedGridSlot.SetItem(newItem);

                itemsToHide.Add(currentItem);
                itemsToShow.Add(newItem);

                ReturnItemToPool(currentItem);
            }

            foreach (var specialItemGridSlot in solvedData.GetSpecialItemGridSlots(true))
            {
                var item = GetItemFromPool();
                item.SetWorldPosition(GetWorldPosition(specialItemGridSlot.GridPosition));

                specialItemGridSlot.SetItem(item);
                itemsToShow.Add(item);
            }

            return new IJob[] { new ItemsHideJob(itemsToHide), new ItemsShowJob(itemsToShow) };
        }
    }
}