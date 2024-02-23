using System;
using UnityEngine;

namespace Match3
{
    public class GameInitMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly AppContext _appContext;
        private readonly IconsSetModel[] _iconSets;
        private readonly GameUiCanvas _gameUiCanvas;

        private bool _isInitialized;

        public GameInitMode(AppContext appContext)
        {
            _appContext = appContext;
            _unityGame = appContext.Resolve<UnityGame>();
            _iconSets = appContext.Resolve<IconsSetModel[]>();
            _gameUiCanvas = appContext.Resolve<GameUiCanvas>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            const int level = 0;

            if (_isInitialized)
            {
                SetLevel(level);
            }
            else
            {
                Init(level);
                SetLevel(level);
            }

            Finished?.Invoke(this, EventArgs.Empty);
        }

        public void Deactivate()
        {
        }

        private void Init(int level)
        {
            var gameBoardData = _appContext.Resolve<IGameBoardDataProvider<IGridSlot>>().GetGameBoardSlots(level);
            var itemGenerator = _appContext.Resolve<IItemsPool<IItem>>();
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;
            itemGenerator.Init(itemsPoolCapacity);
            _isInitialized = true;
        }

        private void SetLevel(int level)
        {
            _unityGame.InitGameLevel(level, _iconSets[_gameUiCanvas.SelectedIconsSetIndex].Sprites);
        }
    }
}