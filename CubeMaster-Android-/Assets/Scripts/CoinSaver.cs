using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public class CoinSaver : MonoBehaviour
{
    void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/CoinsMap/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/CoinsMap/");
        }
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
