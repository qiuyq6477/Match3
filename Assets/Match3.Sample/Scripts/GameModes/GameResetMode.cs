using System;



namespace Match3
{
    public class GameResetMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly IItemsPool<IItem> _itemsPool;
        private readonly UnityGameBoardRenderer _gameBoardRenderer;

        public GameResetMode(AppContext appContext)
        {
            _unityGame = appContext.Resolve<UnityGame>();
            _itemsPool = appContext.Resolve<IItemsPool<IItem>>();
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

        public void Deactivate()
        {
        }
    }
}