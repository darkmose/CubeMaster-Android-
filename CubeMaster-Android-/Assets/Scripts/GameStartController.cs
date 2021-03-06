﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameStartController : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject chooseLevelTypeMenu;
    public GameObject chooseLevelNumberPanel;
    public GameObject optionsMenu;
    public GameObject levelCube;
    public GameObject blackScreen;
    public Text levelTypeText;
    public Button TypeStartButton;
    private Material cube;
    bool rotateCube = false;
    bool changeImage = false;
    int currentLevel = 0;
    public LeveChooseHandler[] levels = new LeveChooseHandler[4];
    public Texture2D defaultNull;
    public GameObject mainCube;
    public int MaxLevels = 12;
    public int countLocations = 4;
    public Text coins;
    AudioManager audioManager;

    AdverManager ad;
    MetaInfo meta;

    private void Start()
    {
        ad = new AdverManager(true);
        audioManager = FindObjectOfType<AudioManager>();
        meta = new MetaInfo(countLocations,MaxLevels);

        if (PlayerPrefs.HasKey("Quality"))
        {
            QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("Quality"), false);
        }
		
		coins.text = SaveManager.coins.ToString();
        int e = Random.Range(1, 3);
        audioManager.Play("Theme" + e.ToString(), AudioManager.sType.music);
        DontDestroyOnLoad(blackScreen);
    }


    public void Sound(string name)
    {
        audioManager.Play(name, AudioManager.sType.sound);
    }    

    public void StartGame(int _сlevel, int indexScene, string name, int countLevels)
    {
        LevelManager.currentMaxLevels = countLevels;
        LevelManager.currentLevel = _сlevel;
        LevelManager.currentIndexLocation = indexScene;
        LevelManager.currentLevelName = name;
        meta.CheckCreateCoinsFiles(_сlevel, indexScene);
        blackScreen.SetActive(true);
        SceneManager.LoadScene(indexScene, LoadSceneMode.Single);
    }

    public void AdvGetCoins()
    {
        if (ad.IsSupported)
        {
            ad.ShowRewardedVideo();
            ad.onEndAd = () => { coins.text = SaveManager.coins.ToString(); }; 
        }        
    }

    public void SkinsShop()
    {
        SceneManager.LoadScene("ShopScene");
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
        optionsMenu.transform.Find("SliderSounds").GetComponent<Slider>().value = FindObjectOfType<AudioManager>().s_volume;
        optionsMenu.transform.Find("SliderMusic").GetComponent<Slider>().value = FindObjectOfType<AudioManager>().m_volume;

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
                FindObjectOfType<AudioManager>().RefreshVolume(
                    optionsMenu.transform.Find("SliderSounds").GetComponent<Slider>().value,
                    optionsMenu.transform.Find("SliderMusic").GetComponent<Slider>().value);                    

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

		int l = chooseLevelNumberPanel.transform.Find("ScrollLevelNumbers").Find("LevelNumbers").childCount;

        for (int i = 0; i < l; i++)
        {
			Destroy(chooseLevelNumberPanel.transform.Find("ScrollLevelNumbers").Find("LevelNumbers").GetChild(i).gameObject);
        }

        for (int k = 0; k < levels[currentLevel].LevelsCount; k++)
        {
            int clevel = (k + 1);

			GameObject number = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Number"), chooseLevelNumberPanel.transform.Find("ScrollLevelNumbers").Find("LevelNumbers"));
            number.transform.GetChild(0).GetComponent<Text>().text = clevel.ToString();
            number.GetComponent<Button>().onClick.RemoveAllListeners();


            number.GetComponent<Button>().onClick.AddListener(() => Sound("Click"));
            number.GetComponent<Button>().onClick.AddListener(delegate () { StartGame(clevel, levels[currentLevel].Index, levels[currentLevel].LevelTypeName, levels[currentLevel].LevelsCount); });
            

            if (SaveManager.scorePerLevels[levels[currentLevel].Index, clevel] != 0)
            {
                for (byte j = 0; j < SaveManager.scorePerLevels[levels[currentLevel].Index, clevel]; j++)
                {
                    Instantiate(Resources.Load<GameObject>("Prefabs/Star"), number.transform.Find("PanelStars"));
                }                
            }           

            if (SaveManager.MaxAviableLevelOnLocation[levels[currentLevel].Index , clevel] == 0)
            {
                number.GetComponent<Button>().interactable = false;
            }
        }
        chooseLevelNumberPanel.transform.Find("levelName").GetComponent<Text>().text = levels[currentLevel].LevelTypeName;
       
        chooseLevelTypeMenu.transform.Find("CubeLevel").gameObject.SetActive(false);
        chooseLevelTypeMenu.transform.Find("Prev").gameObject.SetActive(false);
        chooseLevelTypeMenu.transform.Find("Next").gameObject.SetActive(false);
        chooseLevelTypeMenu.transform.Find("Start").gameObject.SetActive(false);
        chooseLevelTypeMenu.transform.Find("Text").gameObject.SetActive(false);
        chooseLevelNumberPanel.SetActive(true);
        
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
                meta.SaveData();
                Application.Quit();
            }
        }
    }

    IEnumerator RotateY(Quaternion angle)
    {
        rotateCube = true;
        while (levelCube.transform.rotation != angle)
        {
            levelCube.transform.rotation = Quaternion.RotateTowards(levelCube.transform.rotation, angle, Time.fixedDeltaTime * 200);
            yield return new WaitForFixedUpdate();
        }
        rotateCube = false;
    }
    IEnumerator ChangeLevel(Texture2D newTexture)
    {
        changeImage = true;
        cube = chooseLevelTypeMenu.transform.Find("CubeLevel").GetComponent<Renderer>().material;

        while (cube.color.a != 0)
        {        
            cube.color = new Color(cube.color.r, cube.color.g, cube.color.b, Mathf.Clamp01(cube.color.a-0.5f));
            yield return new WaitForFixedUpdate();
        }

        cube.mainTexture = newTexture;

        while (cube.color.a != 1)
        {
            cube.color = new Color(cube.color.r, cube.color.g, cube.color.b, Mathf.Clamp01(cube.color.a + 0.5f));
            yield return new WaitForFixedUpdate();
        }
        changeImage = false;

    }


    public void NextTypeLevel()
    {
        if (!rotateCube && !changeImage)
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
                    chooseLevelTypeMenu.transform.Find("Lock").gameObject.GetComponent<Image>().enabled = false;
                    TypeStartButton.interactable = true;
                    levelTypeText.text = levels[levelType].LevelTypeName;
                    StartCoroutine(ChangeLevel(levels[currentLevel].image));
                }
                else
                {
                    chooseLevelTypeMenu.transform.Find("Lock").gameObject.GetComponent<Image>().enabled = true;
                    TypeStartButton.interactable = false;
                    levelTypeText.text = "LOCKED";
                    StartCoroutine(ChangeLevel(levels[levelType].image));
                }

            }
        }
        catch (System.IndexOutOfRangeException)
        {
            chooseLevelTypeMenu.transform.Find("Lock").gameObject.GetComponent<Image>().enabled = true;
            TypeStartButton.interactable = false;
            levelTypeText.text = "SOON";
            StartCoroutine(ChangeLevel(defaultNull));
        }
       
    }


    private void Update()
    {
        Back();
        if (Input.GetKeyUp(KeyCode.Home))
        {
            meta.SaveData();
        }
    }

}
