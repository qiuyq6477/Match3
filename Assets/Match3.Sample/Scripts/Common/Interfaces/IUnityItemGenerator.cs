using UnityEngine;

namespace Match3
{
    public interface IUnityItemGenerator : IItemGenerator
    {
        void SetSprites(Sprite[] sprites);
    }
}