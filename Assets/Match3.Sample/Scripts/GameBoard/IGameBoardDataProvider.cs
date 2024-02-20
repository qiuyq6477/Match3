using Match3;

namespace Match3
{
    public interface IGameBoardDataProvider<out TGridSlot> where TGridSlot : IGridSlot
    {
        TGridSlot[,] GetGameBoardSlots(int level);
    }
}