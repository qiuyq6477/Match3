using System;

using UnityEngine;

namespace Match3
{
    [Serializable]
    public class TileModel
    {
        [SerializeField] private TileGroup _group;
        [SerializeField] private GameObject _prefab;

        public TileGroup Group => _group;
        public GameObject Prefab => _prefab;
    }
}