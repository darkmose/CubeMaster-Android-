
public static class SaveManager
{
    public static bool canReadWrite = true;
    public static byte IndexOfMaxAviableLocation;
    public static byte[,] MaxAviableLevelOnLocation;
    public static byte[,] scorePerLevels;
}

[System.Serializable]
public class SaveData
{
    public SaveData() { }

    public SaveData(int levelsCount, int maxLevels)
    {
        SaveManager.scorePerLevels = new byte[levelsCount, maxLevels];
        SaveManager.scorePerLevels[1, 1] = 0;
        SaveManager.IndexOfMaxAviableLocation = 1;
        SaveManager.MaxAviableLevelOnLocation = new byte[levelsCount, maxLevels];
        SaveManager.MaxAviableLevelOnLocation[1, 1] = 1;
    }

    public byte IndexOfMaxAviableLocation;
    public byte[,] MaxAviableLevelOnLocation;
    public byte[,] scorePerLevels;
}
