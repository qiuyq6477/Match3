namespace Match3
{
    public interface IGridTilePool<TGridTile> where TGridTile : IGridTile
    {
        TGridTile GetGridTile(TileType tileType);
        void ReturnGridTile(TGridTile gridTile);
    }
}