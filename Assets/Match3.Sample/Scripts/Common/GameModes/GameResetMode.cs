using System;
using Match3;
using Match3;

namespace Match3
{
    public class GameResetMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly IItemsPool<IUnityItem> _itemsPool;
        private readonly UnityGameBoardRenderer _gameBoardRenderer;

        public GameResetMode(AppContext appContext)
        {
            _unityGame = appContext.Resolve<UnityGame>();
            _itemsPool = appContext.Resolve<IItemsPool<IUnityItem>>();
            _gameBoardRenderer = appContext.Resolve<UnityGameBoardRenderer>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            _itemsPool.ReturnAllItems(_unityGame.GetGridSlots());
            _gameBoardRenderer.ResetGridTiles();
            _unityGame.ResetGameBoard();

            Finished?.Invoke(this, EventArgs.Empty);
        }
    }
}