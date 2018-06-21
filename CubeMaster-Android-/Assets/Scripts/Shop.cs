using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
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
    public InventorySkins inventorySkins;
    public GameObject prefabContainer;
    public Material material;
    public Material GameMaterial;
    public GameObject AcceptMenu;

    void Start()
    {
        AcceptMenu.SetActive(false);
        invMenu.GetComponent<RectTransform>().localPosition = Vector2.zero;
        invList.gameObject.SetActive(false);
        openCase.gameObject.SetActive(false);
        invMenu.SetActive(false);
        invMenu.transform.Find("StartRoll").gameObject.SetActive(false);
        InventoryLoad();
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
        for (int i = invList.GetChild(0).childCount - 1; i > 0; i--)
        {
            Destroy(invList.GetChild(0).GetChild(i).gameObject);
        }

        foreach (Skin skin in inventorySkins.skins)
        {
            GameObject prefab = Instantiate(prefabContainer, invList.GetChild(0));
            prefab.GetComponent<InvContainer>().skin = skin;
            switch (skin.rarity)
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


    public void ShowAcceptWindow()
    {
        AcceptMenu.SetActive(true);
    }

    public void SaveChanges()
    {
        GameMaterial.SetTexture("_MainTex", material.GetTexture("_MainTex"));
        if (material.GetTexture("_BumpMap"))
        {
            GameMaterial.SetTexture("_BumpMap", material.GetTexture("_BumpMap"));
        }
        else
        {
            GameMaterial.SetTexture("_BumpMap", null);
        }
        Back();
    }

    void InventoryLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData/Inventory.inv"))
        {
            using (var inv = File.OpenRead(Application.persistentDataPath + "/SaveData/Inventory.inv"))
            {
                var binar = new BinaryFormatter();
                var _inv = (InventoryLoadClass)binar.Deserialize(inv);
                inventory = (InventorySkins)_inv.inventory;
                inv.Close();
            }
        }
        else
        {
            InventorySave();
        }
    }
    void InventorySave()
    {
        using (var inv = File.Open(Application.persistentDataPath + "/SaveData/Inventory.inv", FileMode.Create))
        {
            var binar = new BinaryFormatter();
            var _inv = new InventoryLoadClass(true);
            _inv.inventory = inventory;
            binar.Serialize(inv, _inv);
            inv.Close();
        }
    }
}

[System.Serializable]
public class InventoryLoadClass
{   
    [SerializeField]
    public InventorySkins inventory;

    public InventoryLoadClass(bool isNew)
    {
        inventory = new InventorySkins();
    }
}
