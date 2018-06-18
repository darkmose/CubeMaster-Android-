using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class Shop : MonoBehaviour
{

    public Camera _camera;
    public Transform invList, openCase;
    public GameObject invMenu;
    public Animator anim, cube;
    private bool isOpenSomething = false;
    bool flag = false;
    public InventoryStore inventorySkins;
    public GameObject prefabContainer;

    void Start()
    {
        invMenu.GetComponent<RectTransform>().localPosition = Vector2.zero;
        invList.gameObject.SetActive(false);
        openCase.gameObject.SetActive(false);
        invMenu.SetActive(false);
        invMenu.transform.Find("StartRoll").gameObject.SetActive(false);
    }

    public void Back()
    {
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
        for (int i = invList.childCount - 1; i > 0; i--)
        {
            Destroy(invList.GetChild(i).gameObject);
        }
        int length = inventorySkins.inventory.skins.Count;
        for (int k = 0; k < length; k++)
        {
            GameObject prefab = Instantiate(prefabContainer, invList.GetChild(0));
            prefab.GetComponent<InvContainer>().skin = inventorySkins.inventory.skins[k];
            switch (prefab.GetComponent<InvContainer>().skin.rarity)
            {
                case "Common":
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(26,128,52,80);
                    break;
                case "Uncommon":
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(52, 253, 195, 80);
                    break;
                case "Golden":
                    prefab.transform.Find("background").GetComponent<Image>().color = new Color32(221, 208, 29, 80);
                    break;
                default:
                    prefab.transform.Find("background").GetComponent<Image>().color = Color.black;
                    break;
            }
            prefab.transform.Find("Item").GetComponent<Image>().sprite = inventorySkins.inventory.skins[k].sprite;            
        }
    }

}
