using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    [SerializeField] private Board _boardPrefab;
    [SerializeField] private Hud _hudPrefab;
    [SerializeField] private GameOver _gameOverPrefab;
    [SerializeField] private BoardCell _cellPrefab;
    [SerializeField] private Prize _prizePrefab;

    private Board _board;
    private Hud _hud;
    private GameData _gameData;
    private Pool _pool;
    private GameOver _gameOver;
    private int _boardPrizes;

    public void Init(GameData gameData, Action onRestart)
    {
        _pool = new Pool();
        _gameData = gameData;

        _hud = Instantiate(_hudPrefab);
        _hud.Init(onRestart);
        _hud.SetPrize(PrizeScore());
        _hud.SetScoops(_gameData.LeftScoops);

        _board = Instantiate(_boardPrefab);
        _board.Init(OnCellClick, new Vector2Int(gameData.GameSettings.GreedX, gameData.GameSettings.GreedY),
            _cellPrefab);
        _board.CreateCells(_gameData.Cells);

        _gameOver = Instantiate(_gameOverPrefab);
        _gameOver.Init(onRestart);
        _gameOver.gameObject.SetActive(false);

        StartCoroutine(CreatePrize());
    }

    public void Start()
    {
    }

    private void OnCellClick(int cellId)
    {
        if (NoScoopsNoPrizes())
        {
            GameOver();
            return;
        }

        if (_gameData.Cells[cellId].Depth == 0 || _gameData.LeftScoops == 0 || _gameData.Cells[cellId].HasPrize)
            return;

        _gameData.Cells[cellId].Depth--;
        _hud.SetScoops(--_gameData.LeftScoops);

        _board.UpdateCell(cellId, _gameData.Cells[cellId]);

        var wonPrize = Random.Range(0, 100) <= _gameData.GameSettings.AppearPercent;

        if (wonPrize)
        {
            _gameData.Cells[cellId].HasPrize = true;

            CreatePrize(cellId);
            _boardPrizes++;
        }
    }

    private IEnumerator CreatePrize()
    {
        yield return null;

        for (var i = 0; i < _gameData.Cells.Length; i++)
        {
            if (_gameData.Cells[i].HasPrize)
                CreatePrize(i);
        }
    }

    private void CreatePrize(int cellId)
    {
        var prizeGo = _pool.Get(_prizePrefab.gameObject, transform);
        var prize = prizeGo.GetComponent<Prize>();
        prize.Init(cellId, _hud.Basket, () => OnBasket(prize), _board.CellPosition(cellId), _board.CellSize);
        prizeGo.SetActive(true);
    }

    private void GameOver()
    {
        _gameOver.SetText(Constants.YouLose);
        _gameOver.gameObject.SetActive(true);
    }

    private bool NoScoopsNoPrizes() =>
        _gameData.LeftScoops == 0 && (_gameData.WonPrizes + _boardPrizes) < _gameData.GameSettings.MaxPrizes;
        //((_gameData.LeftScoops + _gameData.WonPrizes + _boardPrizes) < _gameData.GameSettings.MaxPrizes);

    private void OnBasket(Prize prize)
    {
        _gameData.Cells[prize.Id].HasPrize = false;
        prize.OnReturn();
        _pool.Return(prize.gameObject);
        _boardPrizes--;
        _gameData.WonPrizes++;
        _hud.SetPrize(PrizeScore());

        if (_gameData.WonPrizes >= _gameData.GameSettings.MaxPrizes)
        {
            _gameOver.SetText(Constants.YouWon);
            _gameOver.gameObject.SetActive(true);
        }
    }

    private string PrizeScore() =>
        $"{_gameData.WonPrizes} / {_gameData.GameSettings.MaxPrizes}";

    public void Clear()
    {
        if (_board)
            Destroy(_board.gameObject);

        if (_hud)
            Destroy(_hud.gameObject);

        if (_gameOver)
            Destroy(_gameOver.gameObject);
    }
}