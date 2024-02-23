using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Match3
{
    class GameBoard : IGameBoard<IGridSlot>, IDisposable
    {
        private int _rowCount;
        private int _columnCount;

        private IGridSlot[,] _gridSlots;

        public int RowCount => _rowCount;
        public int ColumnCount => _columnCount;

        public IGridSlot this[GridPosition gridPosition] => _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex];
        public IGridSlot this[int rowIndex, int columnIndex] => _gridSlots[rowIndex, columnIndex];

        // public bool IsPositionOnGrid(GridPosition gridPosition)
        // {
        //     EnsureGridSlotsIsNotNull();
        //
        //     return GridMath.IsPositionOnGrid(this, gridPosition);
        // }

        // public bool IsPositionOnBoard(GridPosition gridPosition)
        // {
        //     return IsPositionOnGrid(gridPosition) &&
        //            _gridSlots[gridPosition.RowIndex, gridPosition.ColumnIndex].CanContainItem;
        // }

        public void ResetState()
        {
            _rowCount = 0;
            _columnCount = 0;
            _gridSlots = null;
        }

        public void Dispose()
        {
            if (_gridSlots == null)
            {
                return;
            }

            Array.Clear(_gridSlots, 0, _gridSlots.Length);
            ResetState();
            
            DisposeGridTiles();
            DisposeGameBoardData();
        }


        private float _tileSize = 0.6f;
        private IGridTile[,] _gridTiles;
        private Vector3 _originPosition;
        private IGridTilePool<IGridTile> _gridTilePool;

        
        public void InitGridTiles(int[,] data)
        {
            _rowCount = data.GetLength(0);
            _columnCount = data.GetLength(1);
            _gridTiles = new IGridTile[_rowCount, _columnCount];
            _gridSlots = new IGridSlot[_rowCount, _columnCount];
            _originPosition = GetOriginPosition(_rowCount, _columnCount);

            InitGridTiles(TileType.Available);
        }

        private void InitGridTiles(TileType defaultTileType)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    var gridTile = NewTile(rowIndex, columnIndex, defaultTileType);

                    _gridTiles[rowIndex, columnIndex] = gridTile;
                    _gridSlots[rowIndex, columnIndex] =
                        new GridSlot(gridTile, new GridPosition(rowIndex, columnIndex));
                }
            }
        }
        
        public void SetGridTilePool(IGridTilePool<IGridTile> pool)
        {
            _gridTilePool = pool;
        }

        public bool IsTileAvailable(GridPosition gridPosition)
        {
            return GetTileType(gridPosition) != TileType.Unavailable;
        }

        public void ActivateTile(GridPosition gridPosition)
        {
            SetTileType(gridPosition.RowIndex, gridPosition.ColumnIndex, TileType.Available);
        }

        public void DeactivateTile(GridPosition gridPosition)
        {
            SetTileType(gridPosition.RowIndex, gridPosition.ColumnIndex, TileType.Unavailable);
        }

        public void SetNextGridTileType(GridPosition gridPosition)
        {
            var tileType = GetTileType(gridPosition);
            SetTileType(gridPosition.RowIndex, gridPosition.ColumnIndex, GetNextAvailableGroup(tileType));
        }

        public bool IsPointerOnGrid(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnGrid(gridPosition);
        }

        public bool IsPointerOnBoard(Vector3 worldPointerPosition, out GridPosition gridPosition)
        {
            gridPosition = GetGridPositionByPointer(worldPointerPosition);
            return IsPositionOnBoard(gridPosition);
        }

        public bool IsPositionOnGrid(GridPosition gridPosition)
        {
            return GridMath.IsPositionOnGrid(gridPosition, _rowCount, _columnCount);
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition)
        {
            return GetWorldPosition(gridPosition.RowIndex, gridPosition.ColumnIndex);
        }

        public TileType GetTileType(GridPosition gridPosition)
        {
            return (TileType) _gridTiles[gridPosition.RowIndex, gridPosition.ColumnIndex].TypeId;
        }

        public void ResetGridTiles()
        {
            SetTilesType(TileType.Available);
        }

        public bool IsPositionOnBoard(GridPosition gridPosition)
        {
            return IsPositionOnGrid(gridPosition) && IsTileAvailable(gridPosition);
        }

        private GridPosition GetGridPositionByPointer(Vector3 worldPointerPosition)
        {
            var rowIndex = (worldPointerPosition - _originPosition).y / _tileSize;
            var columnIndex = (worldPointerPosition - _originPosition).x / _tileSize;

            return new GridPosition(Convert.ToInt32(-rowIndex), Convert.ToInt32(columnIndex));
        }

        private Vector3 GetWorldPosition(int rowIndex, int columnIndex)
        {
            return new Vector3(columnIndex, -rowIndex) * _tileSize + _originPosition;
        }

        private Vector3 GetOriginPosition(int rowCount, int columnCount)
        {
            var offsetY = Mathf.Floor(rowCount / 2.0f) * _tileSize;
            var offsetX = Mathf.Floor(columnCount / 2.0f) * _tileSize;

            return new Vector3(-offsetX, offsetY);
        }

        private void SetTilesType(TileType type)
        {
            for (var rowIndex = 0; rowIndex < _rowCount; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < _columnCount; columnIndex++)
                {
                    SetTileType(rowIndex, columnIndex, type);
                }
            }
        }

        private void SetTileType(int rowIndex, int columnIndex, TileType type)
        {
            var currentTile = _gridTiles[rowIndex, columnIndex];
            if (currentTile != null)
            {
                _gridTilePool.ReturnGridTile(currentTile);
            }

            var gridTile = NewTile(rowIndex, columnIndex, type);

            _gridTiles[rowIndex, columnIndex] = gridTile;
            _gridSlots[rowIndex, columnIndex].SetState(gridTile);
        }

        private IGridTile NewTile(int rowIndex, int columnIndex, TileType type)
        {
            var gridTile = _gridTilePool.GetGridTile(type);
            gridTile.SetWorldPosition(rowIndex, columnIndex, GetWorldPosition(rowIndex, columnIndex));

            return gridTile;
        }

        private TileType GetNextAvailableGroup(TileType type)
        {
            var index = (int) type + 1;
            var resultGroup = TileType.Available;
            var groupValues = (TileType[]) Enum.GetValues(typeof(TileType));

            if (index < groupValues.Length)
            {
                resultGroup = groupValues[index];
            }

            return resultGroup;
        }

        private void DisposeGridTiles()
        {
            if (_gridTiles == null)
            {
                return;
            }

            foreach (var gridSlotTile in _gridTiles)
            {
                gridSlotTile.Dispose();
            }

            Array.Clear(_gridTiles, 0, _gridTiles.Length);
            _gridTiles = null;
        }

        private void DisposeGameBoardData()
        {
            if (_gridSlots == null)
            {
                return;
            }

            Array.Clear(_gridSlots, 0, _gridSlots.Length);
            _gridSlots = null;
        }
        
        
        
        
        
        
        
        
        
        
        
        
        
    }
}