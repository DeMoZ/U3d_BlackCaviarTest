using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private Board _boardPrefab;
    [SerializeField] private Hud _hudPrefab;
    [SerializeField] private BoardCell _cellPrefab;
    [SerializeField] private Prize _prizePrefab;

    private Board _board;
    private Hud _hud;
    private GameData _gameData;

    public void Init(GameData gameData)
    {
        _gameData = gameData;

        _hud = Instantiate(_hudPrefab);
        
        _board = Instantiate(_boardPrefab);
        _board.Init(OnCellClick, new Vector2Int(gameData.GameSettings.GreedX, gameData.GameSettings.GreedY),
            _cellPrefab, _prizePrefab, _hud.Basket);
        _board.CreateCells(_gameData.Cells);
    }

    public void Start()
    {
    }

    private void OnCellClick(int cellId)
    {
        if (_gameData.Cells[cellId].Depth == 0) return;


        _gameData.Cells[cellId].Depth--;
        _board.UpdateCell(cellId, _gameData.Cells[cellId]);

        var wonPrize = Random.Range(0, 100) <= _gameData.GameSettings.AppearPercent;
        if (wonPrize)
            _gameData.Cells[cellId].HasPrize = true;
    }


    public void Clear()
    {
        if (_board)
            Destroy(_board.gameObject);

        if (_hud)
            Destroy(_hud.gameObject);
    }
}