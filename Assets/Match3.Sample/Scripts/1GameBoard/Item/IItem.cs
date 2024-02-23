using System;
using Match3;
using UnityEngine;

namespace Match3
{
    public interface IItem : IDisposable
    {
        int ContentId { get; }

        Transform Transform { get; }
        SpriteRenderer SpriteRenderer { get; }

        void Show();
        void Hide();

        void SetSprite(int spriteId, Sprite sprite);
        void SetWorldPosition(Vector3 worldPosition);
        Vector3 GetWorldPosition();
        void SetScale(float value);
    }
}