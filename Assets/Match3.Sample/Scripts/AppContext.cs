using System;
using System.Collections.Generic;
using Match3;
using UnityEngine;

public class AppContext : MonoBehaviour//, IAppContext
{
    [SerializeField] private GameUiCanvas _gameUiCanvas;
    [SerializeField] private CanvasInputSystem _inputSystem;
    [SerializeField] private UnityGameBoardRenderer _gameBoardRenderer;

    [Space]
    [SerializeField] private GameObject _itemPrefab;

    [Space]
    [SerializeField] private IconsSetModel[] _iconSets;

    public void Construct()
    {
        _registeredTypes = new Dictionary<Type, object>();

        RegisterInstance(_inputSystem);
        RegisterInstance(_iconSets);
        RegisterInstance(_gameUiCanvas);
        RegisterInstance<UnityGameBoardRenderer, IGameBoardDataProvider<IUnityGridSlot>>(_gameBoardRenderer);

        RegisterInstance(GetUnityGame());
        RegisterInstance<IUnityItemGenerator, IItemsPool<IUnityItem>>(GetItemGenerator());
        RegisterInstance<IBoardFillStrategy<IUnityGridSlot>[]>(GetBoardFillStrategies());
    }

    private Dictionary<Type, object> _registeredTypes;

    public T Resolve<T>()
    {
        return (T) _registeredTypes[typeof(T)];
    }

    private void RegisterInstance<T>(T instance)
    {
        _registeredTypes.Add(typeof(T), instance);
    }

    private void RegisterInstance<T1, T2>(object instance)
    {
        _registeredTypes.Add(typeof(T1), instance);
        _registeredTypes.Add(typeof(T2), instance);
    }

    private UnityGame GetUnityGame()
    {
        var gameConfig = new GameConfig<IUnityGridSlot>
        {
            GameBoardDataProvider = _gameBoardRenderer,
            ItemSwapper = new AnimatedItemSwapper(),
            LevelGoalsProvider = new LevelGoalsProvider(),
            GameBoardSolver = GetGameBoardSolver(),
            SolvedSequencesConsumers = GetSolvedSequencesConsumers()
        };

        return new UnityGame(_inputSystem, _gameBoardRenderer, gameConfig);
    }

    private UnityItemGenerator GetItemGenerator()
    {
        return new UnityItemGenerator(_itemPrefab, new GameObject("ItemsPool").transform);
    }

    private IGameBoardSolver<IUnityGridSlot> GetGameBoardSolver()
    {
        return new GameBoardSolver<IUnityGridSlot>(GetSequenceDetectors(), GetSpecialItemDetectors());
    }

    private ISequenceDetector<IUnityGridSlot>[] GetSequenceDetectors()
    {
        return new ISequenceDetector<IUnityGridSlot>[]
        {
            new VerticalLineDetector<IUnityGridSlot>(),
            new HorizontalLineDetector<IUnityGridSlot>()
        };
    }

    private ISpecialItemDetector<IUnityGridSlot>[] GetSpecialItemDetectors()
    {
        return new ISpecialItemDetector<IUnityGridSlot>[]
        {
            new StoneItemDetector(),
            new IceItemDetector()
        };
    }

    private ISolvedSequencesConsumer<IUnityGridSlot>[] GetSolvedSequencesConsumers()
    {
        return new ISolvedSequencesConsumer<IUnityGridSlot>[]
        {
            new GameScoreBoard()
        };
    }

    private IBoardFillStrategy<IUnityGridSlot>[] GetBoardFillStrategies()
    {
        return new IBoardFillStrategy<IUnityGridSlot>[]
        {
            new SimpleFillStrategy(this),
            new FallDownFillStrategy(this),
            new SlideDownFillStrategy(this)
        };
    }
}