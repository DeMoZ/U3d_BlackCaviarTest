using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Game _gamePrefab;

    private bool _settingsLoadFinished;
    private bool _dataLoadFinished;

    private GameSettings _settings;
    private GameData _gameData;
    private Game _game;

    private void Start()
    {
        LoadGameData();
        LoadSettings();
    }

    /// <summary>
    /// Delete prefs and Load from settings
    /// </summary>
    public void Restart()
    {
        if (_game)
        {
            _game.Clear();
            Destroy(_game.gameObject);
        }

        StartNewGame();
    }

    private void LoadSettings()
    {
        var loader = new LoadFromStreamingAssets();
        loader.Load(this, Constants.GameSettingsFileName,
            data => OnLoad<GameSettings>(ref _settings, ref _settingsLoadFinished, data),
            () => { Debug.LogError("Settings was not loaded"); });
    }

    private void LoadGameData()
    {
        //var loader = new LoadFromPlayerPrefs();
        var loader = new LoadFromPersistent();
        loader.Load(this, Constants.GameDataFileName,
            data => OnLoad<GameData>(ref _gameData, ref _dataLoadFinished, data),
            () =>
            {
                _dataLoadFinished = true;
                TryLoadSavedGame();
            });
    }

    private void TryLoadSavedGame()
    {
        if (!_settingsLoadFinished || !_dataLoadFinished)
            return;

        if (_settings == null || _settings == new GameSettings())
        {
            Debug.LogError("No Game Rules Loaded");
            return;
        }

        if (_gameData == null || !_gameData.GameSettings.EqualsTo( _settings))
        {
            Debug.Log($"Previously saved gameData not found or is not related to new rules.\nWill start new game");
            _gameData = null;

            StartNewGame();
        }
        else
        {
            LoadSavedGame();
        }
    }

    private void StartNewGame()
    {
        _gameData = new GameData
        {
            LeftScoops = _settings.MaxScoops,
            GameSettings = _settings,
            Cells = CreateCells()
        };

        StartGame();
    }

    private Cell[] CreateCells()
    {
        var totalCells = _settings.GreedX * _settings.GreedY;
        var cells = new Cell[totalCells];
        for (var i = 0; i < totalCells; i++)
        {
            cells[i] = new Cell()
            {
                Depth = _settings.GreedDepth,
                HasPrize = false
            };
        }

        return cells;
    }

    private void LoadSavedGame() =>
        StartGame();

    private void StartGame()
    {
        _game = Instantiate(_gamePrefab);
        _game.Init(_gameData, Restart);
        _game.Start();
    }

    private void OnLoad<T>(ref T model, ref bool isLoadedField, string data)
    {
        try
        {
            model = JsonUtility.FromJson<T>(data);
            isLoadedField = true;
        }
        catch
        {
            model = default;
        }

        TryLoadSavedGame();
    }

    private void OnApplicationQuit() =>
        Save();

    private void Save()
    {
        var gameJson = JsonUtility.ToJson(_gameData);
       Debug.Log(gameJson);

        var saver = new SaveToPersistent();
        //var saver = new SaveToPlayerPrefs();
        saver.Save(gameJson, () => { });
    }
}