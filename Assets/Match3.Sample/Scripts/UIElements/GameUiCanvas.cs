using System;
using System.Linq;





using UnityEngine;

namespace Match3
{
    public class GameUiCanvas : MonoBehaviour//, IGameUiCanvas
    {
        [SerializeField] private AppContext _appContext;
        [SerializeField] private InteractableDropdown _iconsSetDropdown;
        [SerializeField] private InteractableDropdown _fillStrategyDropdown;
        [SerializeField] private InteractableButton _startGameButton;

        public int SelectedIconsSetIndex => _iconsSetDropdown.SelectedIndex;
        public int SelectedFillStrategyIndex => _fillStrategyDropdown.SelectedIndex;

        public event EventHandler StartGameClick;
        public event EventHandler<int> StrategyChanged;

        private void Start()
        {
            _iconsSetDropdown.AddItems(_appContext.Resolve<IconsSetModel[]>().Select(iconsSet => iconsSet.Name));
            _fillStrategyDropdown.AddItems(_appContext.Resolve<IBoardFillStrategy<IGridSlot>[]>()
                .Select(strategy => strategy.Name));
        }

        private void OnEnable()
        {
            _startGameButton.Click += OnStartGameButtonClick;
            _fillStrategyDropdown.IndexChanged += OnFillStrategyDropdownIndexChanged;
        }

        private void OnDisable()
        {
            _startGameButton.Click -= OnStartGameButtonClick;
            _fillStrategyDropdown.IndexChanged -= OnFillStrategyDropdownIndexChanged;
        }

        public void ShowMessage(string message)
        {
            Debug.Log(message);
        }

        public void RegisterAchievedGoal(LevelGoal<IGridSlot> achievedGoal)
        {
            ShowMessage($"The goal {achievedGoal.GetType().Name} achieved.");
        }

        private void OnStartGameButtonClick()
        {
            StartGameClick?.Invoke(this, EventArgs.Empty);
        }

        private void OnFillStrategyDropdownIndexChanged(int index)
        {
            StrategyChanged?.Invoke(this, index);
        }
    }
}