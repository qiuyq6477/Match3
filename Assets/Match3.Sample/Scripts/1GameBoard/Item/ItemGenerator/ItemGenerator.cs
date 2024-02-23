using System;
using System.Collections.Generic;
using Match3;
using Match3;
using UnityEngine;

namespace Match3
{
    public sealed class ItemGenerator :  IItemsPool<IItem>//, IUnityItemGenerator
    {
        private readonly Transform _container;
        private readonly GameObject _itemPrefab;
        private Queue<IItem> _itemsPool;

        public ItemGenerator(GameObject itemPrefab, Transform container)
        {
            _container = container;
            _itemPrefab = itemPrefab;
        }
        
        public void Init(int capacity)
        {
            if (_itemsPool != null)
            {
                throw new InvalidOperationException("Items have already been created.");
            }

            _itemsPool = new Queue<IItem>(capacity);

            for (var i = 0; i < capacity; i++)
            {
                _itemsPool.Enqueue(CreateItem());
            }
        }
        
        private IItem CreateItem()
        {
            var item = _itemPrefab.CreateNew<IItem>(parent: _container);
            item.Hide();

            return item;
        }

        public IItem GetItem()
        {
            return _itemsPool.Dequeue();
        }

        public void ReturnItem(IItem item)
        {
            _itemsPool.Enqueue(item);
        }

        public void Dispose()
        {
            if (_itemsPool == null)
            {
                return;
            }

            foreach (var item in _itemsPool)
            {
                if (item is IDisposable disposable)
                {
                    disposable.Dispose();
                }
                else
                {
                    break;
                }
            }

            _itemsPool.Clear();
            _itemsPool = null;
        }

    }
}