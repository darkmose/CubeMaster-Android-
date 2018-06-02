
public static class SaveManager
{
    public static bool canReadWrite = true;
    public static int IndexOfMaxAviableLocation;
    public static int[,] MaxAviableLevelOnLocation;
    public static int[,] scorePerLevels;
    public static int coins;
}

[System.Serializable]
public class SaveData
{
    public SaveData() { }

    public SaveData(int levelsCount, int maxLevels)
    {
        SaveManager.scorePerLevels = new int[levelsCount, maxLevels];
        SaveManager.IndexOfMaxAviableLocation = 1;
        SaveManager.MaxAviableLevelOnLocation = new int[levelsCount, maxLevels];
        SaveManager.MaxAviableLevelOnLocation[1, 1] = 1;
        SaveManager.coins = 0;
    }

    public int IndexOfMaxAviableLocation;
    public int[,] MaxAviableLevelOnLocation;
    public int[,] scorePerLevels;
    public int coins;
}

