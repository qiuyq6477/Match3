using System;
using Match3;
using UnityEngine;

namespace Match3
{
    public abstract class GridTile : MonoBehaviour, IGridTile
    {
        private bool _isDestroyed;

        public abstract int GroupId { get; }
        public abstract bool IsLocked { get; }
        public abstract bool CanContainItem { get; }

        public int RowIndex;
        public int ColumnIndex;
        
        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }

        public void SetWorldPosition(int rowIndex, int columnIndex, Vector3 worldPosition)
        {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            transform.position = worldPosition;
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
        }

        public void Dispose()
        {
            if (_isDestroyed == false)
            {
                Destroy(gameObject);
            }
        }

        private void OnGUI()
        {
            GUI.Label(new Rect(transform.position, new Vector2(40, 20)), RowIndex + "  " + ColumnIndex);
        }
    }
}