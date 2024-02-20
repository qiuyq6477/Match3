using Match3;
using UnityEngine;

namespace Match3
{
    public class ItemMoveData
    {
        public ItemMoveData(IUnityItem item, Vector3[] worldPositions)
        {
            Item = item;
            WorldPositions = worldPositions;
        }

        public IUnityItem Item { get; }
        public Vector3[] WorldPositions { get; }
    }
}