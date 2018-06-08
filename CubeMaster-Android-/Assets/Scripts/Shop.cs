using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{

    public Camera _camera;
    public Transform invList, openCase;
    public GameObject invMenu;
    public Animator anim;
    private bool isOpenSomething=false;

    void Start()
    {
        invMenu.GetComponent<RectTransform>().localPosition = Vector2.zero;
        invList.gameObject.SetActive(false);
        openCase.gameObject.SetActive(false);
        invMenu.SetActive(false);
    }


    IEnumerator StateInvMenu(string state,RectTransform trans)
    {
        switch (state)
        {
            case "Open":
                Vector2 targetO = new Vector2(0, trans.rect.height);
                while (trans.localPosition.y != targetO.y)
                {
                    trans.localPosition = Vector2.MoveTowards(trans.localPosition, targetO, Time.deltaTime * 25);
                    yield return new WaitForFixedUpdate();
                }
                invMenu.SetActive(true);
                break;

            case "Close":
                Vector2 targetC = Vector2.zero;
                while (trans.localPosition.y != targetC.y)
                {
                    trans.localPosition = Vector2.MoveTowards(trans.localPosition, targetC, Time.deltaTime * 25);
                    yield return new WaitForFixedUpdate();
                }
                invMenu.SetActive(false);
                break;
        }
    }

    void MyInv()
    {
        if (!isOpenSomething || invList.gameObject.activeSelf)
        {
            if (!invList.gameObject.activeSelf)
            {
                isOpenSomething = true;
                anim.SetBool("isShoping", true);
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                invList.gameObject.SetActive(true);
                OpenMyInv();
            }
            else
            {
                isOpenSomething = false;
                anim.SetBool("isShoping", false);
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                invList.gameObject.SetActive(false);
            }
        }        
    }
    
    void OpenCaseInv()
    {
        if (!isOpenSomething || openCase.gameObject.activeSelf)
        {
            if (!openCase.gameObject.activeSelf)
            {
                isOpenSomething = true;
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                openCase.gameObject.SetActive(true);
                OpenCase();
            }
            else
            {
                isOpenSomething = false;
                StartCoroutine(StateInvMenu("Open", invMenu.GetComponent<RectTransform>()));
                openCase.gameObject.SetActive(false);
            }
        }

    }



   void OpenMyInv() { }////////////////////////
    void OpenCase() { }

}
