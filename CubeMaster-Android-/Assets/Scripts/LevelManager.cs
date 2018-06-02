
using System.Collections.Generic;

public static class LevelManager
{
    public static int currentLevel = 0;
    public static int currentIndexLocation = 0;
    public static string currentLevelName = null;
    public static int currentMaxLevels = 0;

    public static List<SerializableVector> coinMaps = null;
}


[System.Serializable]
public class CoinsData
{
    public List<SerializableVector> coinMaps;

    public CoinsData() { }

    public CoinsData(bool isNew)
    {
        coinMaps = new List<SerializableVector>();
    }
}

[System.Serializable]
public class SerializableVector
{
    public float x, z;    
}