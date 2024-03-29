using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Match3
{
    public class DrawGameBoardMode : IGameMode
    {
        private readonly CanvasInputSystem _inputSystem;
        private readonly GameUiCanvas _gameUiCanvas;
        private readonly UnityGameBoardRenderer _gameBoardRenderer;
        private readonly UnityGame _unityGame;
        
        private bool _isDrawMode;
        private bool _isInitialized;
        private GridPosition _previousSlotPosition;

        public DrawGameBoardMode(AppContext appContext)
        {
            _inputSystem = appContext.Resolve<CanvasInputSystem>();
            _gameUiCanvas = appContext.Resolve<GameUiCanvas>();
            _gameBoardRenderer = appContext.Resolve<UnityGameBoardRenderer>();
            _unityGame = appContext.Resolve<UnityGame>();
        }

        public event EventHandler Finished;

        public void Activate()
        {
            if (_isInitialized == false)
            {
                _isInitialized = true;
                var gridTilePool = new GridTilePool(_gameBoardRenderer.GridTilesModel, new GameObject("GridTilePool").transform);
                _unityGame.GameBoard.SetGridTilePool(gridTilePool);
                _unityGame.GameBoard.InitGridTiles(new int[_gameBoardRenderer.RowCount,_gameBoardRenderer.ColumnCount]);
            }

            _inputSystem.PointerDown += OnPointerDown;
            _inputSystem.PointerDrag += OnPointerDrag;
            _inputSystem.PointerUp += OnPointerUp;
            _gameUiCanvas.StartGameClick += OnStartGameClick;
        }

        public void Deactivate()
        {
            _inputSystem.PointerDown -= OnPointerDown;
            _inputSystem.PointerDrag -= OnPointerDrag;
            _inputSystem.PointerUp -= OnPointerUp;
            _gameUiCanvas.StartGameClick -= OnStartGameClick;
        }

        private void OnPointerDown(object sender, PointerEventArgs pointer)
        {
            if (IsPointerOnGrid(pointer.WorldPosition, out var gridPosition) == false)
            {
                return;
            }

            if (IsLeftButton(pointer))
            {
                _isDrawMode = true;
                _previousSlotPosition = gridPosition;

                InvertGridTileState(gridPosition);
            }
            else if (IsRightButton(pointer))
            {
                SetNextGridTileGroup(gridPosition);
            }
        }

        private void OnPointerDrag(object sender, PointerEventArgs pointer)
        {
            if (_isDrawMode == false)
            {
                return;
            }

            if (IsPointerOnGrid(pointer.WorldPosition, out var slotPosition) == false)
            {
                return;
            }

            if (IsSameSlot(slotPosition))
            {
                return;
            }

            _previousSlotPosition = slotPosition;
            InvertGridTileState(slotPosition);
        }

        private void OnPointerUp(object sender, PointerEventArgs pointer)
        {
            _isDrawMode = false;
        }

        private void OnStartGameClick(object sender, EventArgs e)
        {
            Finished?.Invoke(this, EventArgs.Empty);
        }

        private bool IsLeftButton(PointerEventArgs pointer)
        {
            return pointer.Button == PointerEventData.InputButton.Left;
        }

        private bool IsRightButton(PointerEventArgs pointer)
        {
            return pointer.Button == PointerEventData.InputButton.Right;
        }

        private bool IsPointerOnGrid(Vector3 worldPosition, out GridPosition gridPosition)
        {
            return _unityGame.GameBoard.IsPointerOnGrid(worldPosition, out gridPosition);
        }

        private bool IsSameSlot(GridPosition slotPosition)
        {
            return _previousSlotPosition.Equals(slotPosition);
        }

        private void InvertGridTileState(GridPosition gridPosition)
        {
            if (_unityGame.GameBoard.IsTileAvailable(gridPosition))
            {
                _unityGame.GameBoard.DeactivateTile(gridPosition);
            }
            else
            {
                _unityGame.GameBoard.ActivateTile(gridPosition);
            }
        }

        private void SetNextGridTileGroup(GridPosition gridPosition)
        {
            if (_unityGame.GameBoard.IsTileAvailable(gridPosition))
            {
                _unityGame.GameBoard.SetNextGridTileType(gridPosition);
            }
        }
    }
}