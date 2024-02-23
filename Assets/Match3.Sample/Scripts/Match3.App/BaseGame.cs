using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using Match3;
using Match3;
using Match3;
using UnityEngine;

namespace Match3
{
    public abstract class BaseGame<TGridSlot> : IDisposable where TGridSlot : IGridSlot
    {
        private readonly GameBoard<TGridSlot> _gameBoard;
        private readonly IGameBoardSolver<TGridSlot> _gameBoardSolver;
        private readonly ILevelGoalsProvider<TGridSlot> _levelGoalsProvider;
        private readonly IGameBoardDataProvider<TGridSlot> _gameBoardDataProvider;
        private readonly ISolvedSequencesConsumer<TGridSlot>[] _solvedSequencesConsumers;
        
        private readonly JobsExecutor _jobsExecutor;
        private readonly IItemSwapper<TGridSlot> _itemSwapper;

        private AsyncLazy _swapItemsTask;
        private IBoardFillStrategy<TGridSlot> _fillStrategy;

        private bool _isStarted;
        private int _achievedGoals;

        private LevelGoal<TGridSlot>[] _levelGoals;

        private Sprite[] _sprites;
        
        protected BaseGame(GameConfig<TGridSlot> config)
        {
            _gameBoard = new GameBoard<TGridSlot>();

            _gameBoardSolver = config.GameBoardSolver;
            _levelGoalsProvider = config.LevelGoalsProvider;
            _gameBoardDataProvider = config.GameBoardDataProvider;
            _solvedSequencesConsumers = config.SolvedSequencesConsumers;
            _itemSwapper = config.ItemSwapper;
            _jobsExecutor = new JobsExecutor();
        }

        protected IGameBoard<TGridSlot> GameBoard => _gameBoard;

        protected bool IsSwapItemsCompleted
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _swapItemsTask == null || _swapItemsTask.Task.Status.IsCompleted();
        }
        public event EventHandler Finished;
        public event EventHandler<LevelGoal<TGridSlot>> LevelGoalAchieved;

        public void InitGameLevel(int level, Sprite[] sprites)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Can not be initialized while the current game is active.");
            }

            _sprites = sprites;
            _gameBoard.SetGridSlots(_gameBoardDataProvider.GetGameBoardSlots(level));
            _levelGoals = _levelGoalsProvider.GetLevelGoals(level, _gameBoard);
        }

        public Sprite[] GetSprite()
        {
            return _sprites;
        }
        
        public void SetGameBoardFillStrategy(IBoardFillStrategy<TGridSlot> fillStrategy)
        {
            _fillStrategy = fillStrategy;
        }

        public async UniTask StartAsync(CancellationToken cancellationToken = default)
        {
            if (_isStarted)
            {
                throw new InvalidOperationException("Game has already been started.");
            }

            if (_fillStrategy == null)
            {
                throw new NullReferenceException(nameof(_fillStrategy));
            }

            await FillAsync(_fillStrategy, cancellationToken);

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved += OnLevelGoalAchieved;
            }

            _isStarted = true;
            OnGameStarted();
        }

        public async UniTask StopAsync()
        {
            if (_isStarted == false)
            {
                throw new InvalidOperationException("Game has not been started.");
            }
            
            if (IsSwapItemsCompleted == false)
            {
                await _swapItemsTask;
            }

            foreach (var levelGoal in _levelGoals)
            {
                levelGoal.Achieved -= OnLevelGoalAchieved;
            }

            _isStarted = false;
            OnGameStopped();
        }
        
        public void ResetGameBoard()
        {
            _achievedGoals = 0;
            _gameBoard.ResetState();
        }

        public void Dispose()
        {
            _gameBoard?.Dispose();
        }

        protected abstract void OnGameStarted();
        protected abstract void OnGameStopped();

        protected bool IsSolved(GridPosition position1, GridPosition position2, out SolvedData<TGridSlot> solvedData)
        {
            solvedData = _gameBoardSolver.Solve(GameBoard, position1, position2);
            return solvedData.SolvedSequences.Count > 0;
        }

        protected void NotifySequencesSolved(SolvedData<TGridSlot> solvedData)
        {
            foreach (var sequencesConsumer in _solvedSequencesConsumers)
            {
                sequencesConsumer.OnSequencesSolved(solvedData);
            }

            foreach (var levelGoal in _levelGoals)
            {
                if (levelGoal.IsAchieved == false)
                {
                    levelGoal.OnSequencesSolved(solvedData);
                }
            }
        }

        protected virtual void OnAllGoalsAchieved()
        {
            Finished?.Invoke(this, EventArgs.Empty);
            RaiseGameFinishedAsync().Forget();
        }

        private void OnLevelGoalAchieved(object sender, EventArgs e)
        {
            LevelGoalAchieved?.Invoke(this, (LevelGoal<TGridSlot>) sender);

            _achievedGoals++;
            if (_achievedGoals == _levelGoals.Length)
            {
                OnAllGoalsAchieved();
            }
        }
        
        protected UniTask SwapItemsAsync(GridPosition position1, GridPosition position2,
            CancellationToken cancellationToken = default)
        {
            if (_swapItemsTask?.Task.Status.IsCompleted() ?? true)
            {
                _swapItemsTask = SwapItemsAsync(_fillStrategy, position1, position2, cancellationToken).ToAsyncLazy();
            }

            return _swapItemsTask.Task;
        }

        protected virtual async UniTask SwapItemsAsync(IBoardFillStrategy<TGridSlot> fillStrategy, GridPosition position1,
            GridPosition position2, CancellationToken cancellationToken = default)
        {
            await SwapGameBoardItemsAsync(position1, position2, cancellationToken);

            if (IsSolved(position1, position2, out var solvedData))
            {
                NotifySequencesSolved(solvedData);
                await ExecuteJobsAsync(fillStrategy.GetSolveJobs(GameBoard, solvedData), cancellationToken);
            }
            else
            {
                await SwapGameBoardItemsAsync(position1, position2, cancellationToken);
            }
        }

        protected async UniTask SwapGameBoardItemsAsync(GridPosition position1, GridPosition position2,
            CancellationToken cancellationToken = default)
        {
            var gridSlot1 = GameBoard[position1.RowIndex, position1.ColumnIndex];
            var gridSlot2 = GameBoard[position2.RowIndex, position2.ColumnIndex];

            await _itemSwapper.SwapItemsAsync(gridSlot1, gridSlot2, cancellationToken);
        }

        protected UniTask ExecuteJobsAsync(IEnumerable<IJob> jobs, CancellationToken cancellationToken = default)
        {
            return _jobsExecutor.ExecuteJobsAsync(jobs, cancellationToken);
        }

        private async UniTask RaiseGameFinishedAsync()
        {
            if (IsSwapItemsCompleted == false)
            {
                await _swapItemsTask;
            }

            OnAllGoalsAchieved();
        }
        
        private async UniTask FillAsync(IBoardFillStrategy<TGridSlot> fillStrategy,
            CancellationToken cancellationToken = default)
        {
            await ExecuteJobsAsync(fillStrategy.GetFillJobs(GameBoard), cancellationToken);
        }
    }
}