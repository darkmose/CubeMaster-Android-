using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameStartController : MonoBehaviour
{

    public GameObject mainMenu;
    public GameObject chooseLevelTypeMenu;
    public GameObject chooseLevelTypeButtons;
    public GameObject chooseLevelNumberPanel;
    public GameObject optionsMenu;

    public void StartGame(string level, int indexScene, string name)
    {
        PlayerPrefs.SetString("LevelPrefabId", level);
        PlayerPrefs.SetString("levelName", name);
        SceneManager.LoadScene(indexScene, LoadSceneMode.Single);
    }

    public void LevelType()
    {
        mainMenu.SetActive(false);
        chooseLevelTypeMenu.SetActive(true);
    }

    public void OptionsMenu()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
    }

    public void BackToMain()
    {
        if (chooseLevelNumberPanel.activeSelf)
        {
            chooseLevelNumberPanel.SetActive(false);
            chooseLevelTypeButtons.SetActive(true);
        }
        else
        {
            if (optionsMenu.activeSelf)
            {
                optionsMenu.SetActive(false);
                mainMenu.SetActive(true);

            }
            else
            {
                chooseLevelTypeMenu.SetActive(false);
                mainMenu.SetActive(true);
            }

        }
    }

    public void QualityChange(int q)
    {
        QualitySettings.SetQualityLevel(q, false);
    }

    public void ChooseLevelNumber(LeveChooseHandler handler)
    {

        int l = chooseLevelNumberPanel.transform.GetChild(0).childCount;

        for (int i = 0; i < l; i++)
        {
            Destroy(chooseLevelNumberPanel.transform.GetChild(0).GetChild(i).gameObject);
        }

        for (int i = 0; i < handler.LevelsCount; i++)
        {
            GameObject number = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Number"), chooseLevelNumberPanel.transform.Find("LevelNumbers"));
            number.transform.GetChild(0).GetComponent<Text>().text = (i + 1).ToString();
            string nameLevel = handler.LevelsIndex.ToString() + "-" + (i + 1).ToString();
            number.GetComponent<Button>().onClick.AddListener(delegate () { StartGame(nameLevel, handler.LevelsIndex, handler.LevelTypeName); });
        }
        chooseLevelNumberPanel.transform.Find("levelName").GetComponent<Text>().text = handler.LevelTypeName;
        chooseLevelNumberPanel.transform.Find("levelNameB").GetComponent<Text>().text = handler.LevelTypeName;
        chooseLevelNumberPanel.SetActive(true);
        chooseLevelTypeButtons.SetActive(false);

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
                Application.Quit();
            }
        }
    }

    private void Update()
    {
        Back();
    }
}
