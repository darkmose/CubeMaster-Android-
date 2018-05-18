using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.IO;

public class GameStartController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject chooseLevelTypeMenu;
    public GameObject chooseLevelNumberPanel;
    public GameObject optionsMenu;
    public GameObject levelCube;
    public Text levelTypeText;
    public Button TypeStartButton;
    private Material cube;
    bool rotateCube = false;
    int currentLevel = 0;
    public LeveChooseHandler[] levels = new LeveChooseHandler[4];
    public Texture2D defaultNull;
    public GameObject mainCube;
    public int MaxLevels = 12;
    SaveData save;
    string path;
    public int countLocations = 4;

    private void Start()
    {
        if (!Directory.Exists(Application.persistentDataPath + "/SaveData/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/SaveData/");
        }
        path = Application.persistentDataPath + "/SaveData/" + "saveData.sv";

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), false);
        }
       

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
                    fs.Close();
                    SaveManager.canReadWrite = false;

                }
            }            
        }
        else if(SaveManager.canReadWrite)
        {
            save = new SaveData(levels.Length,MaxLevels);

            save.IndexOfMaxAviableLocation = SaveManager.IndexOfMaxAviableLocation;
            save.MaxAviableLevelOnLocation = SaveManager.MaxAviableLevelOnLocation;
            save.scorePerLevels = SaveManager.scorePerLevels;

            using (FileStream fs = File.Open(path, FileMode.CreateNew))
            {
                BinaryFormatter binary = new BinaryFormatter();
                binary.Serialize(fs, save);
                fs.Close();
            }
        }
    }

    public void StartGame(int _сlevel, int indexScene, string name, int countLevels)
    {
        LevelManager.currentMaxLevels = countLevels;
        LevelManager.currentLevel = _сlevel;
        print(LevelManager.currentLevel);
        LevelManager.currentIndexLocation = indexScene;
        LevelManager.currentLevelName = name;
        SceneManager.LoadScene(indexScene, LoadSceneMode.Single);
    }
    
    public void LevelType()
    {
        mainMenu.SetActive(false);
        mainCube.SetActive(false);
        chooseLevelTypeMenu.SetActive(true);
        currentLevel = 0;
        TypeStartButton.enabled = true;
        levelTypeText.text = levels[currentLevel].LevelTypeName;
        chooseLevelTypeMenu.transform.Find("CubeLevel").GetComponent<Renderer>().material.mainTexture = levels[0].image;
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        if (PlayerPrefs.HasKey("Quality"))
        {
            optionsMenu.transform.Find("Dropdown").GetComponent<Dropdown>().value = PlayerPrefs.GetInt("Quality");
        }
        mainMenu.SetActive(false);
    }

    public void BackToMain()
    {
        if (chooseLevelNumberPanel.activeSelf)
        {
            chooseLevelNumberPanel.SetActive(false);
            chooseLevelTypeMenu.transform.Find("CubeLevel").gameObject.SetActive(true);
            chooseLevelTypeMenu.transform.Find("Prev").gameObject.SetActive(true);
            chooseLevelTypeMenu.transform.Find("Next").gameObject.SetActive(true);
            chooseLevelTypeMenu.transform.Find("Start").gameObject.SetActive(true);
            chooseLevelTypeMenu.transform.Find("Text").gameObject.SetActive(true);
        }
        else
        {
            if (optionsMenu.activeSelf)
            {
                PlayerPrefs.SetInt("Quality", optionsMenu.transform.Find("Dropdown").GetComponent<Dropdown>().value);
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);
            }
            else if (chooseLevelTypeMenu.activeSelf)
            {
                chooseLevelTypeMenu.SetActive(false);
                mainCube.SetActive(true);
                mainMenu.SetActive(true);
            }
        }
    }

    public void QualityChange(int q)
    {
        QualitySettings.SetQualityLevel(q, false);
    }

    public void ChooseLevelNumber()
    {

        int l = chooseLevelNumberPanel.transform.GetChild(0).childCount;

        for (int i = 0; i < l; i++)
        {
            Destroy(chooseLevelNumberPanel.transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int k = 0; k < levels[currentLevel].LevelsCount; k++)
        {
            GameObject number = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Number"), chooseLevelNumberPanel.transform.Find("LevelNumbers"));
            number.transform.GetChild(0).GetComponent<Text>().text = (k + 1).ToString();
            number.GetComponent<Button>().onClick.RemoveAllListeners();
            int clevel = k + 1;

            number.GetComponent<Button>().onClick.AddListener(delegate () { StartGame(clevel, levels[currentLevel].Index, levels[currentLevel].LevelTypeName, levels[currentLevel].LevelsCount); });
            if (SaveManager.scorePerLevels[levels[currentLevel].Index, k + 1] != 0)
            {
                for (byte j = 0; j < SaveManager.scorePerLevels[levels[currentLevel].Index, k + 1]; j++)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/Star"), number.transform.Find("PanelStars"));
                }                
            }           

            if (SaveManager.MaxAviableLevelOnLocation[levels[currentLevel].Index , k + 1] == 0)
            {
                number.GetComponent<Button>().interactable = false;
            }
        }
        chooseLevelNumberPanel.transform.Find("levelName").GetComponent<Text>().text = levels[currentLevel].LevelTypeName;
        chooseLevelNumberPanel.transform.Find("levelNameB").GetComponent<Text>().text = levels[currentLevel].LevelTypeName;
        {
            chooseLevelTypeMenu.transform.Find("CubeLevel").gameObject.SetActive(false);
            chooseLevelTypeMenu.transform.Find("Prev").gameObject.SetActive(false);
            chooseLevelTypeMenu.transform.Find("Next").gameObject.SetActive(false);
            chooseLevelTypeMenu.transform.Find("Start").gameObject.SetActive(false);
            chooseLevelTypeMenu.transform.Find("Text").gameObject.SetActive(false);
            chooseLevelNumberPanel.SetActive(true);
        }
    }

    void Back()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (chooseLevelTypeMenu.activeSelf)
            {
                BackToMain();
            }
            else
            {
                SaveAllData();
                Application.Quit();
            }
        }
    }

    void SaveAllData()
    {
        save = new SaveData
        {
            IndexOfMaxAviableLocation = SaveManager.IndexOfMaxAviableLocation,
            MaxAviableLevelOnLocation = SaveManager.MaxAviableLevelOnLocation,
            scorePerLevels = SaveManager.scorePerLevels
        };

        using (FileStream fs = File.Open(path, FileMode.Create))
        {
            BinaryFormatter binary = new BinaryFormatter();
            binary.Serialize(fs, save);
            fs.Close();
        }
        SaveManager.canReadWrite = true;
    }

    IEnumerator RotateY(Quaternion angle)
    {
        rotateCube = true;
        while (levelCube.transform.rotation != angle)
        {
            levelCube.transform.rotation = Quaternion.RotateTowards(levelCube.transform.rotation, angle, Time.deltaTime * 200);
            yield return null;
        }
        rotateCube = false;
    }
    IEnumerator ChangeLevel(Texture2D newTexture)
    {

        cube = chooseLevelTypeMenu.transform.Find("CubeLevel").GetComponent<Renderer>().material;

        while (cube.color.a != 0)
        {        
            cube.color = new Color(1, 1, 1, Mathf.Clamp01(cube.color.a-0.7f));
            yield return null;
        }

        cube.mainTexture = newTexture;

        while (cube.color.a != 1)
        {
            cube.color = new Color(1, 1, 1, Mathf.Clamp01(cube.color.a + 0.055f));
            yield return null;
        }
    }


    public void NextTypeLevel()
    {
        if (!rotateCube)
        {
            if (currentLevel < countLocations-1)
            {
                currentLevel++;
            }
            else
            {
                currentLevel = 0;
            }
            ChangeLevelType(currentLevel);

            Quaternion quaternion = Quaternion.AngleAxis(90, Vector3.up);
            StartCoroutine(RotateY(quaternion * levelCube.transform.rotation));
        }

    }

    public void PrevTypeLevel()
    {
        if (!rotateCube)
        {
            if (currentLevel > 0)
            {
                currentLevel--;
            }
            else
            {
                currentLevel = countLocations - 1;
            }
            ChangeLevelType(currentLevel);

            Quaternion quaternion = Quaternion.AngleAxis(-90, Vector3.up);
            StartCoroutine(RotateY(quaternion * levelCube.transform.rotation));
        }        
    }

    void ChangeLevelType(int levelType)
    {
        try
        {
            if (levels[levelType] != null)
            {
                
                if (levels[levelType].Index <= SaveManager.IndexOfMaxAviableLocation)
                {
                    TypeStartButton.interactable = true;
                    levelTypeText.text = levels[levelType].LevelTypeName;
                    StartCoroutine(ChangeLevel(levels[currentLevel].image));
                }
                else
                {
                    TypeStartButton.interactable = false;
                    levelTypeText.text = "LOCKED";
                    StartCoroutine(ChangeLevel(levels[levelType].image));
                }

            }
        }
        catch (System.IndexOutOfRangeException)
        {
            TypeStartButton.interactable = false;
            levelTypeText.text = "SOON";
            StartCoroutine(ChangeLevel(defaultNull));
        }
       
    }


    private void Update()
    {
        Back();
    }

}
