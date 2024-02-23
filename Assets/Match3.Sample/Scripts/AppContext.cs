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
        RegisterInstance<UnityGameBoardRenderer, IGameBoardDataProvider<IGridSlot>>(_gameBoardRenderer);

        RegisterInstance(GetUnityGame());
        RegisterInstance<IItemsPool<IItem>>(GetItemGenerator());
        RegisterInstance<IBoardFillStrategy<IGridSlot>[]>(GetBoardFillStrategies());
    }

    public void Destruct()
    {
        var unityGame = Resolve<UnityGame>();
        var itemGenerator = Resolve<IItemsPool<IItem>>();
        unityGame.Dispose();
        itemGenerator.Dispose();
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
        var gameConfig = new GameConfig<IGridSlot>
        {
            GameBoardDataProvider = _gameBoardRenderer,
            ItemSwapper = new AnimatedItemSwapper(),
            LevelGoalsProvider = new LevelGoalsProvider(),
            GameBoardSolver = GetGameBoardSolver(),
            SolvedSequencesConsumers = GetSolvedSequencesConsumers()
        };

        return new UnityGame(_inputSystem, _gameBoardRenderer, gameConfig);
    }

    private ItemGenerator GetItemGenerator()
    {
        return new ItemGenerator(_itemPrefab, new GameObject("ItemsPool").transform);
    }

    private IGameBoardSolver<IGridSlot> GetGameBoardSolver()
    {
        return new GameBoardSolver<IGridSlot>(GetSequenceDetectors(), GetSpecialItemDetectors());
    }

    private ISequenceDetector<IGridSlot>[] GetSequenceDetectors()
    {
        return new ISequenceDetector<IGridSlot>[]
        {
            new VerticalLineDetector<IGridSlot>(),
            new HorizontalLineDetector<IGridSlot>()
        };
    }

    private ISpecialItemDetector<IGridSlot>[] GetSpecialItemDetectors()
    {
        return new ISpecialItemDetector<IGridSlot>[]
        {
            new StoneItemDetector(),
            new IceItemDetector()
        };
    }

    private ISolvedSequencesConsumer<IGridSlot>[] GetSolvedSequencesConsumers()
    {
        return new ISolvedSequencesConsumer<IGridSlot>[]
        {
            new GameScoreBoard()
        };
    }

    private IBoardFillStrategy<IGridSlot>[] GetBoardFillStrategies()
    {
        return new IBoardFillStrategy<IGridSlot>[]
        {
            new SimpleFillStrategy(this),
            new FallDownFillStrategy(this),
            new SlideDownFillStrategy(this)
        };
    }
}