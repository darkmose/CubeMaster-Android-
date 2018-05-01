using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour
{
    public GameObject menu;
    public GameObject winScreen;
    CameraMove _camera;

    
    private void Start()
    {
        _camera = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        GameObject.Find("Main Camera").transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.RemoveAllListeners();
        GameObject.Find("Main Camera").transform.Find("Canvas").Find("Button").GetComponent<Button>().onClick.AddListener(delegate () { ButtonBack(); });
        menu.SetActive(false);
        LoadPrefabOnLevel();
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

    public void Win()
    {
        winScreen.SetActive(true);
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
