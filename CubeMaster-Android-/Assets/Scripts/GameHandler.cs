using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject winScreen;
    public bool loadLevel = true;
    public Stars stars;
    public GameObject starsPanel;
    public GameObject advMenu;
    public Text coins;
    public byte mplevel;
    public string soundTheme;
    AudioManager audioManager;
    MetaInfo meta;
   

    private void Awake()
    {        
        audioManager = FindObjectOfType<AudioManager>();
        meta = new MetaInfo();
    }

    public void Sound(string name)
    {
        audioManager.Play(name, AudioManager.sType.sound);
    }

    private void Start()
    {       
        coins.text = SaveManager.coins.ToString();
        if (loadLevel)
        {
            string level = LevelManager.currentIndexLocation.ToString() + "-" + LevelManager.currentLevel.ToString();
            LoadPrefabOnLevel(level);
        }    
        advMenu.SetActive(true);
        audioManager.Play(soundTheme, AudioManager.sType.music);
    }

    void Coins()
    {
        meta.SaveCoins();
    }

    void NextCoinMap()
    {
        meta.CheckCreateCoinsFiles(LevelManager.currentLevel, LevelManager.currentIndexLocation);
    }

    public void LoadPrefabOnLevel(string prefabName)
    {
        try
        {            
            Instantiate(Resources.Load<GameObject>("LevelPrefabs/"+ prefabName));
            Destroy(GameObject.Find("LoadScreen"));
        }
        catch (System.NullReferenceException)
        {
            ToMainMenu(); //Correct this later; To choose level number
        }
        catch (System.ArgumentException)
        {
            ToMainMenu();
        }
    }

    public void NextLevel()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject.FindGameObjectWithTag("End").transform.Find("WinPart").GetChild(i).gameObject.SetActive(false);
        }
        advMenu.SetActive(true);
        advMenu.GetComponent<AdvMenu>().lvlName.text = advMenu.GetComponent<AdvMenu>().Name;
        GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().StopAllCoroutines();
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        LevelManager.currentLevel++;
        string prefabName = LevelManager.currentIndexLocation.ToString() + "-" + (LevelManager.currentLevel).ToString();
        NextCoinMap();
        LoadPrefabOnLevel(prefabName);
        winScreen.SetActive(false);      
    }

    IEnumerator InstStars()
    {
        for (int i = 0; i < starsPanel.transform.childCount; i++)
        {
            Destroy(starsPanel.transform.GetChild(i).gameObject);
        }        
        for (int i = 0; i < stars.GetResult(); i++)
        {
            GameObject newStar = Instantiate(Resources.Load<GameObject>("Prefabs/Star"), starsPanel.transform);
            newStar.transform.localScale = Vector3.zero;
            while (newStar.transform.localScale.x < 1)
            {
                newStar.transform.localScale += Vector3.one * 0.1f;
                yield return new WaitForSeconds(0.005f);
            }
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void Win()
    {
        advMenu.SetActive(false);
        winScreen.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            GameObject.FindGameObjectWithTag("End").transform.Find("WinPart").GetChild(i).gameObject.SetActive(true);
        }

        Coins();

        StartCoroutine(InstStars());

        if (SaveManager.scorePerLevels[LevelManager.currentIndexLocation, LevelManager.currentLevel] != 0)
        {
            if (stars.GetResult() > SaveManager.scorePerLevels[LevelManager.currentIndexLocation, LevelManager.currentLevel])
            {
                SaveManager.scorePerLevels[LevelManager.currentIndexLocation, LevelManager.currentLevel] = stars.GetResult();
            }
        }
        else
        {
            SaveManager.scorePerLevels[LevelManager.currentIndexLocation, LevelManager.currentLevel] = stars.GetResult();
        }

        if (LevelManager.currentLevel < LevelManager.currentMaxLevels)
        {
            SaveManager.MaxAviableLevelOnLocation[LevelManager.currentIndexLocation, (LevelManager.currentLevel + 1)] = 1;
        }

        if (LevelManager.currentLevel == LevelManager.currentMaxLevels)
        {
            SaveManager.IndexOfMaxAviableLocation = LevelManager.currentIndexLocation + 1;
            SaveManager.MaxAviableLevelOnLocation[SaveManager.IndexOfMaxAviableLocation, 1] = 1;

            winScreen.transform.Find("Panel").Find("NextLevel").GetComponent<Button>().interactable = false;
        }
    }

    public void RetryLevel()
    {
        MainCube mainCubeScript = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
        GameObject _MainCube = GameObject.FindGameObjectWithTag("MainCube");

        StopAllCoroutines();

        if (mainCubeScript.canMove)
        {
            if (_MainCube.name == "SecondCube")
            {
                mainCubeScript.ChangeCube(mainCubeScript.Main, mainCubeScript.Second);
            }
            _MainCube.transform.position = GameObject.FindGameObjectWithTag("Start").transform.GetChild(0).position;
            _MainCube.transform.rotation = mainCubeScript.Orig;
        }

        OnRetryLevelEvent();

        if (winScreen.activeSelf) winScreen.SetActive(false);
        advMenu.GetComponent<AdvMenu>().AnimatePanel("Close");
        stars.countCur = 0;
        Time.timeScale = 1;
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
		SaveManager.coins -= mplevel;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Home))
        {
            SaveManager.coins -= mplevel;
            meta.SaveData();
        }
    }



    public event System.EventHandler OnRetryLevel;

    protected virtual void OnRetryLevelEvent()
    {
        if (OnRetryLevel != null)
        {
            OnRetryLevel(this, System.EventArgs.Empty);
        }             
    }      

}
