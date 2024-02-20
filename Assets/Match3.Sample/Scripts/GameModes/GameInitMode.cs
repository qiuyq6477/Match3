using System;
using UnityEngine;

namespace Match3
{
    public class GameInitMode : IGameMode, IDisposable
    {
        private readonly UnityGame _unityGame;
        private readonly AppContext _appContext;
        private readonly IconsSetModel[] _iconSets;
        private readonly GameUiCanvas _gameUiCanvas;
        private readonly IUnityItemGenerator _itemGenerator;

        private bool _isInitialized;

        public GameInitMode(AppContext appContext)
        {
            _appContext = appContext;
            _unityGame = appContext.Resolve<UnityGame>();
            _iconSets = appContext.Resolve<IconsSetModel[]>();
            _gameUiCanvas = appContext.Resolve<GameUiCanvas>();
            _itemGenerator = appContext.Resolve<IUnityItemGenerator>();
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

        public void Dispose()
        {
            _unityGame.Dispose();
            _itemGenerator.Dispose();
        }

        private void Init(int level)
        {
            var gameBoardData = _appContext.Resolve<IGameBoardDataProvider<IUnityGridSlot>>().GetGameBoardSlots(level);
            var rowCount = gameBoardData.GetLength(0);
            var columnCount = gameBoardData.GetLength(1);
            var itemsPoolCapacity = rowCount * columnCount + Mathf.Max(rowCount, columnCount) * 2;

            _itemGenerator.CreateItems(itemsPoolCapacity);
            _isInitialized = true;
        }

        private void SetLevel(int level)
        {
            _unityGame.InitGameLevel(level);
            _itemGenerator.SetSprites(_iconSets[_gameUiCanvas.SelectedIconsSetIndex].Sprites);
        }
    }
}