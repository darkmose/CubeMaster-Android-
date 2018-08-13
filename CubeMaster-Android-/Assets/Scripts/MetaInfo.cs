using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;


public class MetaInfo
{
    SaveData save;
    string path;
    int MaxLevels;
    int countLocations;

    public MetaInfo() { }
    public MetaInfo(int _cl, int _ml)
    {
        MaxLevels = _ml;
        countLocations = _cl;

        CheckCreateDir();
        CheckCreateSaveFiles();
    }

    void CheckCreateDir()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/");
        }
        if (!Directory.Exists(Application.persistentDataPath + "/CoinsMap/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/CoinsMap/");
        }
    }
    void CheckCreateSaveFiles()
    {

        path = Application.persistentDataPath + "/SaveData/" + "saveData.sv";

        if (File.Exists(path))
        {
            if (SaveManager.canReadWrite)
            {
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    BinaryFormatter binary = new BinaryFormatter();
                    save = (SaveData)binary.Deserialize(fs);
                    SaveManager.IndexOfMaxAviableLocation = save.IndexOfMaxAviableLocation;
                    SaveManager.MaxAviableLevelOnLocation = save.MaxAviableLevelOnLocation;
                    SaveManager.scorePerLevels = save.scorePerLevels;
                    SaveManager.coins = save.coins;
                    fs.Close();
                    SaveManager.canReadWrite = false;
                }
            }
        }
        else if (SaveManager.canReadWrite)
        {
            save = new SaveData(countLocations, MaxLevels)
            {
                IndexOfMaxAviableLocation = SaveManager.IndexOfMaxAviableLocation,
                MaxAviableLevelOnLocation = SaveManager.MaxAviableLevelOnLocation,
                scorePerLevels = SaveManager.scorePerLevels,
                coins = SaveManager.coins
            };

            using (FileStream fs = File.Open(path, FileMode.CreateNew))
            {
                BinaryFormatter binary = new BinaryFormatter();
                binary.Serialize(fs, save);
                fs.Close();
                SaveManager.canReadWrite = false;
            }
        }
    }

    public void SaveData()
    {
        path = Application.persistentDataPath + "/SaveData/" + "saveData.sv";

        save = new SaveData
        {
            IndexOfMaxAviableLocation = SaveManager.IndexOfMaxAviableLocation,
            MaxAviableLevelOnLocation = SaveManager.MaxAviableLevelOnLocation,
            scorePerLevels = SaveManager.scorePerLevels,
            coins = SaveManager.coins
        };

        using (FileStream fs = File.Open(path, FileMode.Create))
        {
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(fs, save);
            fs.Close();
        }
        SaveManager.canReadWrite = true;
    }

    public void CheckCreateCoinsFiles(int _сlevel, int indexScene)
    {
        string coinMapPath = Application.persistentDataPath + "/CoinsMap/" + LevelManager.currentIndexLocation.ToString() + "-" + LevelManager.currentLevel.ToString() + ".svm";
        if (File.Exists(coinMapPath))
        {
            using (FileStream fs = File.Open(coinMapPath, FileMode.Open))
            {
                BinaryFormatter binary = new BinaryFormatter();
                CoinsData coins = (CoinsData)binary.Deserialize(fs);
                LevelManager.coinMaps = coins.coinMaps;
                fs.Close();
            }
        }
        else
        {
            using (FileStream fs = File.Create(coinMapPath))
            {
                BinaryFormatter binary = new BinaryFormatter();
                CoinsData coins = new CoinsData(true);
                LevelManager.coinMaps = coins.coinMaps;
                binary.Serialize(fs, coins);
                fs.Close();
            }
        }
    }

    public void SaveCoins()
    {
        string coinMapPath = Application.persistentDataPath + "/CoinsMap/" + LevelManager.currentIndexLocation.ToString() + "-" + LevelManager.currentLevel.ToString() + ".svm";

        using (FileStream fs = File.Open(coinMapPath, FileMode.Create))
        {
            BinaryFormatter binary = new BinaryFormatter();
            CoinsData coins = new CoinsData
            {
                coinMaps = LevelManager.coinMaps
            };
            LevelManager.coinMaps = null;
            binary.Serialize(fs, coins);
            fs.Close();
        }
    }

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
