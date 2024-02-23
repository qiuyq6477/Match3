using System;
using Match3;
using Cysharp.Threading.Tasks;

namespace Match3
{
    public class GamePlayMode : IGameMode
    {
        private readonly UnityGame _unityGame;
        private readonly GameUiCanvas _gameUiCanvas;
        private readonly IBoardFillStrategy<IGridSlot>[] _boardFillStrategies;

        public GamePlayMode(AppContext appContext)
        {
            _unityGame = appContext.Resolve<UnityGame>();
            _gameUiCanvas = appContext.Resolve<GameUiCanvas>();
            _boardFillStrategies = appContext.Resolve<IBoardFillStrategy<IGridSlot>[]>();
        }

        public event EventHandler Finished
        {
            add => _unityGame.Finished += value;
            remove => _unityGame.Finished -= value;
        }

        public void Activate()
        {
            _unityGame.LevelGoalAchieved += OnLevelGoalAchieved;
            _gameUiCanvas.StrategyChanged += OnStrategyChanged;

            _unityGame.SetGameBoardFillStrategy(GetSelectedFillStrategy());
            _unityGame.StartAsync().Forget();

            _gameUiCanvas.ShowMessage("Game started.");
        }

        public void Deactivate()
        {
            _unityGame.LevelGoalAchieved -= OnLevelGoalAchieved;
            _gameUiCanvas.StrategyChanged -= OnStrategyChanged;

            _unityGame.StopAsync().Forget();
            _gameUiCanvas.ShowMessage("Game finished.");
        }

        private void OnLevelGoalAchieved(object sender, LevelGoal<IGridSlot> levelGoal)
        {
            _gameUiCanvas.RegisterAchievedGoal(levelGoal);
        }

        private void OnStrategyChanged(object sender, int index)
        {
            _unityGame.SetGameBoardFillStrategy(GetFillStrategy(index));
        }

        private IBoardFillStrategy<IGridSlot> GetSelectedFillStrategy()
        {
            return GetFillStrategy(_gameUiCanvas.SelectedFillStrategyIndex);
        }

        private IBoardFillStrategy<IGridSlot> GetFillStrategy(int index)
        {
            return _boardFillStrategies[index];
        }
    }
}