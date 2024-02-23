using System;

using UnityEngine;
using UnityEngine.Serialization;

namespace Match3
{
    [Serializable]
    public class TileModel
    {
        [FormerlySerializedAs("_group")] [SerializeField] private TileType type;
        [SerializeField] private GameObject _prefab;

        public TileType Type => type;
        public GameObject Prefab => _prefab;
    }
}