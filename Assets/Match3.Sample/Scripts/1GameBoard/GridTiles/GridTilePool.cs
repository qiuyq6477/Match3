using System.Collections.Generic;
using UnityEngine;

namespace Match3
{
    public class GridTilePool : IGridTilePool<IGridTile>
    {
        private readonly Transform _itemsContainer;
        private readonly Dictionary<TileType, GameObject> _tilePrefabs;
        private readonly Dictionary<TileType, Queue<IGridTile>> _itemsPool;

        public GridTilePool(IReadOnlyCollection<TileModel> tiles, Transform itemsContainer)
        {
            _itemsContainer = itemsContainer;
            _itemsPool = new Dictionary<TileType, Queue<IGridTile>>(tiles.Count);
            _tilePrefabs = new Dictionary<TileType, GameObject>(tiles.Count);

            foreach (var tile in tiles)
            {
                _tilePrefabs.Add(tile.Type, tile.Prefab);
                _itemsPool.Add(tile.Type, new Queue<IGridTile>());
            }
        }

        public IGridTile GetGridTile(TileType tileType)
        {
            var tiles = _itemsPool[tileType];
            var gridTile = tiles.Count == 0 ? CreateTile(_tilePrefabs[tileType]) : tiles.Dequeue();
            gridTile.SetActive(true);

            return gridTile;
        }

        public void ReturnGridTile(IGridTile gridTile)
        {
            if (gridTile is IStatefulSlot statefulSlot)
            {
                statefulSlot.ResetState();
            }

            gridTile.SetActive(false);
            _itemsPool[(TileType) gridTile.TypeId].Enqueue(gridTile);
        }

        private IGridTile CreateTile(GameObject tilePrefab)
        {
            return tilePrefab.CreateNew<IGridTile>(parent: _itemsContainer);
        }
    }
}