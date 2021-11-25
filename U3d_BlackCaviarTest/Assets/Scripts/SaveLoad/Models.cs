[System.Serializable]
public class GameSettings
{
    public float AppearPercent;
    public int MaxPrizes;
    public int MaxScoops;

    public int GreedX;
    public int GreedY;
    public int GreedDepth;
}

[System.Serializable]
public class GameData
{
    public GameSettings GameSettings;

    public int WonPrizes;
    public int LeftScoops;

    public Cell[] Cells;
}

[System.Serializable]
public class Cell
{
    public int Depth;
    public bool HasPrize;
}
