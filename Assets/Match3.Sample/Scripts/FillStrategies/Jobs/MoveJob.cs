using System.Runtime.CompilerServices;
using DG.Tweening;
using Match3;
using Match3;

namespace Match3
{
    public abstract class MoveJob : Job
    {
        private const float MoveDuration = 0.25f;

        protected MoveJob(int executionOrder) : base(executionOrder)
        {
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected Tween CreateItemMoveTween(ItemMoveData data)
        {
            return data.Item.Transform.DOPath(data.WorldPositions, MoveDuration);
        }
    }
}