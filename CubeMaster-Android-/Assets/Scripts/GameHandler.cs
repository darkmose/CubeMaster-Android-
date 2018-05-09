using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

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
        GameObject.Find("Main Camera").transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(delegate () { ButtonBack(); });
        menu.SetActive(false);
        if (loadLevel)
        {
            LoadPrefabOnLevel();
        }
        
    }

    public void ButtonBack()
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

    public void LoadPrefabOnLevel()
    {
        try
        {
            Instantiate(Resources.Load<GameObject>("LevelPrefabs/" + PlayerPrefs.GetString("LevelPrefabId")));
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
        int level;
        int index;
        string LevelName;

        string str = PlayerPrefs.GetString("LevelPrefabId");

        index = System.Convert.ToInt32(str[0].ToString());

        if (str.Length == 3)
            level = System.Convert.ToInt32(str[2].ToString()) + 1;
        else
            level = System.Convert.ToInt32(str[2].ToString() + str[3].ToString()) + 1;

        LevelName = index.ToString() + "-" + level.ToString();
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        
        PlayerPrefs.SetString("LevelPrefabId", LevelName);
        print(PlayerPrefs.GetString("LevelPrefabId"));
        LoadPrefabOnLevel();
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
                yield return new WaitForSeconds(0.01f);
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SaveResultInMemory()
    {
        FileStream fileStream = new FileStream("Data/saves.dss", FileMode.OpenOrCreate);
        //fileStream;
    }

    public void Win()
    {
        winScreen.SetActive(true);

        for (int i = 0; i < 3; i++)
        {
            GameObject.FindGameObjectWithTag("End").transform.Find("WinPart").GetChild(i).gameObject.SetActive(true);
        }
        StartCoroutine(InstStars());
    }

    public void RetryLevel()
    {
        Destroy(GameObject.FindGameObjectWithTag("Level"));
        Instantiate<GameObject>(Resources.Load<GameObject>("LevelPrefabs/" + PlayerPrefs.GetString("LevelPrefabId")));
        winScreen.SetActive(false);
    }

    void OpenGameMenu()
    {
        Time.timeScale = 0;
        menu.SetActive(true);
        menu.transform.Find("levelName").GetComponent<Text>().text = PlayerPrefs.GetString("levelName");
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
