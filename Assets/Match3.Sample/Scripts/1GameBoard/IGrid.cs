namespace Match3
{
    public interface IGrid
    {
        int RowCount { get; }
        int ColumnCount { get; }

        bool IsPositionOnGrid(GridPosition gridPosition);
    }
}