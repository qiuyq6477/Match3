
using UnityEngine;

namespace Match3
{
    public interface IGameBoard<TGridSlot> : IGrid where TGridSlot : IGridSlot
    {
        // void SetGridSlots(TGridSlot[,] gridSlots);
        void SetGridTilePool(IGridTilePool<IGridTile> pool);

        TGridSlot this[GridPosition gridPosition] { get; }
        TGridSlot this[int rowIndex, int columnIndex] { get; }


        void InitGridTiles(int[,] data);
        void ResetGridTiles();
        bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsPositionOnBoard(GridPosition gridPosition);
        bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition);
        bool IsTileAvailable(GridPosition gridPosition);
        void DeactivateTile(GridPosition gridPosition);
        void ActivateTile(GridPosition gridPosition);
        void SetNextGridTileType(GridPosition gridPosition);
        Vector3 GetWorldPosition(GridPosition gridPosition);

        void ResetState();
        
        void Dispose();
    }
}