using System;
using Match3;
using UnityEngine;
using UnityEngine.U2D;

namespace Match3
{
    [Serializable]
    public class IconsSetModel
    {
        [SerializeField] private string _name;
        [SerializeField] private SpriteAtlas _spriteAtlas;

        public string Name => _name;
        public Sprite[] Sprites => _spriteAtlas.GetSprites();
    }
}