using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    [SerializeField] private Game _gamePrefab;

    private bool _settingsLoaded;
    private bool _dataLoaded;

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
            //(data) => OnLoad<GameSettings>(ref _settings, ref _settingsLoaded, data),
            OnLoadSettings,
            () => { });
    }

    private void LoadGameData()
    {
        var loader = new LoadFromPlayerPrefs();
        loader.Load(this, Constants.GameDataFileName,
            (data) => OnLoad<GameData>(ref _gameData, ref _dataLoaded, data),
            () => { });
    }

    private void TryLoadSavedGame()
    {
        if (!_settingsLoaded || !_dataLoaded)
            return;

        if (_settings == null)
        {
            Debug.LogError("No Game Rules Loaded");
            return;
        }
        
        Debug.Log(_settings);

        if (_gameData == null || _gameData.GameSettings != _settings)
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
            GameSettings = _settings
        };

        StartGame();
    }

    private void LoadSavedGame()
    {
        StartGame();
    }

    private void StartGame()
    {
        _game = Instantiate(_gamePrefab);
        _game.Init(_gameData);
        _game.Start();
    }


    private void OnLoad<T>(ref T model, ref bool isLoadedField, string data)
    {
        try
        {
            model = JsonUtility.FromJson<T>(data);
        }
        catch
        {
            model = default;
        }
        finally
        {
            isLoadedField = true;
            TryLoadSavedGame();
        }

        
    }
private void OnLoadSettings( string data)
    {
        try
        {
            _settings = JsonUtility.FromJson<GameSettings>(data);
        }
        catch
        {
            _settings = default;
        }
        finally
        {
            _settingsLoaded = true;
            TryLoadSavedGame();
        }

        
    }

    private void OnApplicationQuit() =>
        Save();

    private void Save()
    {
    }

    /*private void Save()
    {
        var game = new GameData();
        game.GameSettings = new GameSettings();
        var gameJson = JsonUtility.ToJson(game);
        Debug.Log(gameJson);

        var saver = new SaveToPersistent();
        saver.Save(gameJson, () => { });
    }*/
}