using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Match3
{
    public class UnityGameBoardRenderer : MonoBehaviour//, IGameBoardDataProvider<IGridSlot>// , IUnityGameBoardRenderer
    {
        [FormerlySerializedAs("_rowCount")] [Space]
        public int RowCount = 9;
        [FormerlySerializedAs("_columnCount")] public int ColumnCount = 9;

        [Space]
        [SerializeField] private float _tileSize = 0.6f;

        [FormerlySerializedAs("_gridTilesModel")]
        [Space]
        public TileModel[] GridTilesModel;

       
    }
}