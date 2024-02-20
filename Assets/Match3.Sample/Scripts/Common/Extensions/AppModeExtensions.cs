using System;

namespace Match3
{
    public static class AppModeExtensions
    {
        public static void Deactivate(this IGameMode gameMode)
        {
            if (gameMode is IDeactivatable deactivatable)
            {
                deactivatable.Deactivate();
            }
        }

        public static void Dispose(this IGameMode gameMode)
        {
            if (gameMode is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }
}