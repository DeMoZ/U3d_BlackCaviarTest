using UnityEngine;

public class Game : MonoBehaviour
{
    //private GameData _gameData;

    [SerializeField] private Board _boardPrefab;
    [SerializeField] private Hud _hudPrefab;
    [SerializeField] private BoardCell _cellPrefab;
    
    private Board _board;
    private Hud _hud;

    public void Init(GameData gameData)
    {
        //_gameData = gameData;

        _board = Instantiate(_boardPrefab);
        _board.Init(gameData, _cellPrefab);
        
        
        _hud = Instantiate(_hudPrefab);
    }

    public void Start()
    {
        
    }

    public void Clear()
    {
        if (_board)
            Destroy(_board.gameObject);

        if (_hud)
            Destroy(_hud.gameObject);
    }
}