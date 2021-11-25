using System;
using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;

    private BoardCell[] _boardCells;
    private Prize _prizePrefab;
    private Vector2 _cellSize;
    private RectTransform _basket;

    public void Init(Action<int> onCellClick, Vector2Int gridSize, BoardCell cellPrefab, Prize prizePrefab,
        RectTransform basket)
    {
        _prizePrefab = prizePrefab;
        _basket = basket;
        var gridT = _grid.transform as RectTransform;

        _cellSize.x = gridT.rect.width / gridSize.x;
        _cellSize.y = gridT.rect.height / gridSize.y;

        _grid.constraintCount = gridSize.x;
        _grid.cellSize = _cellSize;

        var totalCells = gridSize.x * gridSize.y;
        _boardCells = new BoardCell[totalCells];

        for (var i = 0; i < totalCells; i++)
        {
            var id = i;
            _boardCells[i] = Instantiate(cellPrefab, _grid.transform);
            _boardCells[i].OnClick(() => { onCellClick?.Invoke(id); });
        }
    }

    public void CreateCells(Cell[] cells)
    {
        for (var i = 0; i < _boardCells.Length; i++)
        {
            _boardCells[i].UpdateCell(cells[i].Depth);
        }
    }

    public void UpdateCell(int id, Cell cell)
    {
        _boardCells[id].UpdateCell(cell.Depth);

        if (cell.HasPrize)
        {
            var prize = Instantiate(_prizePrefab, transform);
            var prizeT = prize.transform as RectTransform;
            prizeT.sizeDelta = _cellSize;
            prize.Init(id,(_boardCells[id].transform as RectTransform).position, _basket);
        }
    }
}