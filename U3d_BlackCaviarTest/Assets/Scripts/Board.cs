using UnityEngine;
using UnityEngine.UI;

public class Board : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _grid;
    public void Init(GameData gameData, BoardCell cellPrefab)
    {
        Debug.Log($"init board");
        _grid.constraintCount = gameData.GameSettings.GreedX;
        var totalCells = gameData.GameSettings.GreedX * gameData.GameSettings.GreedY;

        for (int i = 0; i < totalCells; i++)
        {
            Instantiate(cellPrefab,_grid.transform);
        }
        
    }
}