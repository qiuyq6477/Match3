namespace Match3
{
    public interface IGameBoardDataProvider<out TGridSlot> where TGridSlot : IGridSlot
    {
        TGridSlot[,] GetGameBoardSlots(int level);

        TileModel[] GetTileModel();
    }
}