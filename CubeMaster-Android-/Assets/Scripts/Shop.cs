using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class Shop : MonoBehaviour
{
    public InventorySkins inventory;
    public Camera _camera;
    public Transform invList, openCase;
    public GameObject invMenu;
    public Animator anim, cube;
    private bool isOpenSomething = false;
    bool flag = false;
    public GameObject prefabContainer;
    public Material material;
    public GameObject AcceptMenu;
    public SkinsHolder skinsHolder;
    public Skin finishSkin;

    void Start()
    {
        finishSkin = null;
        inventory = new InventorySkins();
        AcceptMenu.SetActive(false);
        invMenu.GetComponent<RectTransform>().localPosition = Vector2.zero;
        invList.gameObject.SetActive(false);
        openCase.gameObject.SetActive(false);
        invMenu.SetActive(false);
        invMenu.transform.Find("StartRoll").gameObject.SetActive(false);
        InventoryLoad();
        SetShopMaterial();
    }

    public void Back()
    {
        InventorySave();
        SceneManager.LoadScene(0);
    }

    IEnumerator StateInvMenu(string state, RectTransform trans)
    {
        back:
        if (!flag)
        {
            flag = true;

            switch (state)
            {

                case "Open":
                    invMenu.SetActive(true);
                    trans.anchoredPosition = new Vector2(0, 0);
                    Vector2 targetO = new Vector2(0, trans.rect.height);
                    while (trans.anchoredPosition.y != targetO.y)
                    {
                        trans.anchoredPosition = Vector2.MoveTowards(trans.anchoredPosition, targetO, 20);
                        yield return new WaitForFixedUpdate();
                    }
                    flag = false;
                    break;

                case "Close":
                    Vector2 targetC = Vector2.zero;
                    trans.anchoredPosition = new Vector2(0, trans.rect.height);
                    while (trans.anchoredPosition.y != targetC.y)
                    {
                        trans.anchoredPosition = Vector2.MoveTowards(trans.anchoredPosition, targetC, 20);
                        yield return new WaitForFixedUpdate();
                    }
                    invMenu.SetActive(false);
                    flag = false;
                    break;
            }
        }
        else
        {
            while (flag) { yield return new WaitForFixedUpdate(); }
            goto back;
        }

    }

    public void MyInv()
    {
        if (!isOpenSomething || invList.gameObject.activeSelf)
        {
            if (!invList.gameObject.activeSelf)
            {
                isOpenSomething = true;
                anim.SetBool("isShoping", true);
                cube.SetBool("Rotate", true);
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                invList.gameObject.SetActive(true);
                OpenMyInv();
            }
            else
            {
                isOpenSomething = false;
                anim.SetBool("isShoping", false);
                cube.SetBool("Rotate", false);
                StartCoroutine(StateInvMenu("Close", invMenu.GetComponent<RectTransform>()));
                invList.gameObject.SetActive(false);
            }
        }
        else
        {
            OpenCaseInv();
            isOpenSomething = true;
            anim.SetBool("isShoping", true);
            cube.SetBool("Rotate", true);
            StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
            invList.gameObject.SetActive(true);
            OpenMyInv();
        }
    }
    public void OpenCaseInv()
    {
        if (!isOpenSomething || openCase.gameObject.activeSelf)
        {
            if (!openCase.gameObject.activeSelf)
            {
                isOpenSomething = true;
                anim.SetBool("isCases", true);
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                openCase.gameObject.SetActive(true);
                invMenu.transform.Find("StartRoll").gameObject.SetActive(true);
            }
            else
            {
                isOpenSomething = false;
                anim.SetBool("isCases", false);
                StartCoroutine(StateInvMenu("Close", invMenu.GetComponent<RectTransform>()));
                openCase.gameObject.SetActive(false);
                invMenu.transform.Find("StartRoll").gameObject.SetActive(false);

            }
        }
        else
        {
            MyInv();
            isOpenSomething = true;
            anim.SetBool("isCases", true);
            StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
            openCase.gameObject.SetActive(true);
            invMenu.transform.Find("StartRoll").gameObject.SetActive(true);
        }

    }

    void OpenMyInv()
    {
        for (int i = invList.GetChild(0).childCount - 1; i >= 0; i--)
        {
            Destroy(invList.GetChild(0).GetChild(i).gameObject);
        }

        foreach (Skin skin in inventory.skins)
        {
            GameObject prefab = Instantiate(prefabContainer, invList.GetChild(0));
            prefab.GetComponent<InvContainer>().skin = skin;

            switch (prefab.GetComponent<InvContainer>().skin.rarity)
            {
                case Skin.Rarity.Common:
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(26, 128, 52, 80);
                    break;
                case Skin.Rarity.Uncommon:
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(52, 253, 195, 80);
                    break;
                case Skin.Rarity.Golden:
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(221, 208, 29, 80);
                    break;
                default:
                    prefab.transform.Find("background").GetComponent<Image>().color = Color.black;
                    break;
            }
            prefab.transform.Find("Item").GetComponent<Image>().sprite = skin.sprite;
        }
    }

    

    public void CloseAcceptWindow()
    {
        AcceptMenu.SetActive(false);
    }

    public void ShowAcceptWindow()
    {
        AcceptMenu.SetActive(true);
    }



    Skin TakeSkinFromPacks(Skin.Rarity rarity, string name)
    {
        Skin skin;
        switch (rarity)
        {
            case Skin.Rarity.Common:
                skin = skinsHolder.skinPacks[0].skins.Find(x => x.name == name);
                return skin;

            case Skin.Rarity.Uncommon:
                skin = skinsHolder.skinPacks[1].skins.Find(x => x.name == name);
                return skin;

            case Skin.Rarity.Golden:
                skin = skinsHolder.skinPacks[2].skins.Find(x => x.name == name);
                return skin;
        }

        return null;
    }

    void InventoryLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData/Inventory.json"))
        {
            string[] jasonTex = File.ReadAllLines(Application.persistentDataPath + "/SaveData/Inventory.json");
            for (int i = 0; i < jasonTex.Length;)
            {
                Skin.Rarity _rarity = (Skin.Rarity)(System.Convert.ToInt32(jasonTex[i++]));
                string _name = jasonTex[i++];
                if (_name == "Stone")
                {
                    inventory.skins.Add(Resources.Load<Skin>("Prefabs/Stone"));
                    continue;
                }
                inventory.skins.Add(TakeSkinFromPacks(_rarity, _name));
            }
        }
        else
        {
            inventory.skins.Add(Resources.Load<Skin>("Prefabs/Stone"));
            InventorySave();
        }
    }

    public void SaveChanges()
    {
        if (finishSkin)
        {
            using (var fs = File.Create(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv")) { fs.Close(); }
            string[] info = new string[2];
            info[0] = ((int)finishSkin.rarity).ToString();
            info[1] = finishSkin.name;
            File.WriteAllLines(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv", info);
        }
        Back();
    }


    void SetShopMaterial()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv"))
        {
            string[] info = new string[2];
            info = File.ReadAllLines(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv");
            Skin.Rarity _rarity = (Skin.Rarity)(System.Convert.ToInt32(info[0]));
            string _name = info[1];

            if (_name == "Stone")
            {
                material.SetTexture("_MainTex", Resources.Load<Skin>("Prefabs/Stone").sprite.texture);
                material.SetTexture("_BumpMap", Resources.Load<Skin>("Prefabs/Stone").normalSprite);
                return;
            }

            Skin skin = TakeSkinFromPacks(_rarity, _name);
            material.SetTexture("_MainTex", skin.sprite.texture);
            if (skin.normalSprite)
            {
                material.SetTexture("_BumpMap", skin.normalSprite);
            }
        }
        else
        {
            material.SetTexture("_MainTex", Resources.Load<Skin>("Prefabs/Stone").sprite.texture);
            material.SetTexture("_BumpMap", Resources.Load<Skin>("Prefabs/Stone").normalSprite);
        }
    }

    void InventorySave()
    {
        File.WriteAllText(Application.persistentDataPath + "/SaveData/Inventory.json", "");

        byte bi = 0;
        string[] jsonText = new string[inventory.skins.Count * 2];
        foreach (var item in inventory.skins)
        {
            jsonText[bi++] = ((int)item.rarity).ToString();
            jsonText[bi++] = item.name;
        }
        File.WriteAllLines(Application.persistentDataPath + "/SaveData/Inventory.json", jsonText);
    }
}
