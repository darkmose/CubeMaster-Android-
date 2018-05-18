using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject winScreen;
    CameraMove _camera;
    public bool loadLevel = true;
    public Stars stars;
    public GameObject starsPanel;

    private void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        GameObject.Find("Main Camera").transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Main Camera").transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(delegate () { ButtonBackCube(); });
        menu.SetActive(false);
        if (loadLevel)
        {
            string level = LevelManager.currentIndexLocation.ToString() +"-"+ LevelManager.currentLevel.ToString();
            LoadPrefabOnLevel(level);
        }        
    }

    public void ButtonBackCube()
    {
		if(!winScreen.activeSelf)
		{
			MainCube mainCubeScript = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
			GameObject _MainCube = GameObject.FindGameObjectWithTag("MainCube");

			if (mainCubeScript.canMove)
			{
				if (_MainCube.name == "SecondCube")
				{
					mainCubeScript.ChangeCube(mainCubeScript.Main, mainCubeScript.Second);
				}
				_MainCube.transform.position = GameObject.FindGameObjectWithTag("Start").transform.GetChild(0).position;
				_MainCube.transform.rotation = mainCubeScript.Orig;
			}
		}
    }

    public void LoadPrefabOnLevel(string prefabName)
    {
        try
        {
            Instantiate(Resources.Load<GameObject>("LevelPrefabs/" + prefabName));
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
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        LevelManager.currentLevel++;
        string prefabName = LevelManager.currentIndexLocation.ToString() + "-" + (LevelManager.currentLevel).ToString();
        LoadPrefabOnLevel(prefabName);
		GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().RefreshCube();
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
        winScreen.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            GameObject.FindGameObjectWithTag("End").transform.Find("WinPart").GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(InstStars());

        SaveManager.scorePerLevels[LevelManager.currentIndexLocation, LevelManager.currentLevel] = (byte)stars.GetResult();

        if (LevelManager.currentLevel < LevelManager.currentMaxLevels)
        {
            SaveManager.MaxAviableLevelOnLocation[LevelManager.currentIndexLocation, LevelManager.currentLevel + 1] = 1;
        }
        if (LevelManager.currentLevel == LevelManager.currentMaxLevels)
        {
            SaveManager.IndexOfMaxAviableLocation = System.Convert.ToByte(LevelManager.currentIndexLocation + 1);
            winScreen.transform.Find("Panel").Find("NextLevel").GetComponent<Button>().interactable = false;
        }
    }

    public void RetryLevel()
    {
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        string levelName = LevelManager.currentIndexLocation.ToString() + LevelManager.currentLevel.ToString();
        LoadPrefabOnLevel(levelName);
        GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().RefreshCube();
        winScreen.SetActive(false);
    }

    void OpenGameMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        string levelName = LevelManager.currentLevelName + "\n" + LevelManager.currentIndexLocation.ToString() + "-" + LevelManager.currentLevel.ToString();
        menu.transform.Find("Buttons").Find("levelName").GetComponent<Text>().text = levelName;
    }

    public void CloseGameMenu()
    {
        Time.timeScale = 1;
        menu.SetActive(false);
    }

    public void ToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 0 && !winScreen.activeSelf)
                CloseGameMenu();
            else if (Time.timeScale == 1 && !winScreen.activeSelf)
                OpenGameMenu();
            else if (winScreen.activeSelf)
                ToMainMenu();
        }
    }
}
