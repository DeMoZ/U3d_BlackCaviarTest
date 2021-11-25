using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    public void Init(GameData gameData, BoardCell cellPrefab)
    {
        var gridT = _grid.transform as RectTransform;
        var totalCells = gameData.GameSettings.GreedX * gameData.GameSettings.GreedY;
        Vector2 cellSize ;
        cellSize.x = gridT.rect.width / gameData.GameSettings.GreedX;
        cellSize.y = gridT.rect.height / gameData.GameSettings.GreedY;
        _grid.constraintCount = gameData.GameSettings.GreedX;
        _grid.cellSize = cellSize;

        for (int i = 0; i < totalCells; i++)
        {
            var cell = Instantiate(cellPrefab,_grid.transform);
            cell.Init(gameData.Cells[i]);
            //cell.OnClick(() => UpdateCell(cell));
        }
    }

    private void UpdateCell(BoardCell cell)
    {
        
        if (cell.HasPrise)
        {
            // collect prize
            
            // Also need to support drug
        }
        else
        {
            
        }
    }
}