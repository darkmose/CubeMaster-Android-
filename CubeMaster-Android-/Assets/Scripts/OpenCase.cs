using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCase : MonoBehaviour
{

    public Case[] cases;
    bool isRoll = false;
    bool isPrize = false;
    public GameObject casePrefab;
    public Transform casePanel;
    byte speedRoll = 3;
    float velocity;
    public Transform center, prizePanel;
    [HideInInspector]
    public CaseHolder result;

    public void StartRoll()
    {
        if (!isRoll && !isPrize)
        {
            GenerateCases();
            casePanel.position = center.position;
            speedRoll = (byte)Random.Range(5, 6.5f);
            velocity = Random.Range(-25f, -28f);
            isRoll = true;
        }
    }

    void GenerateCases()
    {
        for (int i = casePanel.childCount - 1; i >= 0; i--)
        {
            Destroy(casePanel.GetChild(i).gameObject);
        }
        for (int i = 0; i < 70; i++)
        {
            int rand = Random.Range(0, 1000);
            byte index = 11;
            if (rand <= 500)
            {
                index = 0;
            }
            else if (rand > 500 && rand <= 850)
            {
                index = 1;
            }
            else
            {
                index = 2;
            }

            GameObject _case = Instantiate(casePrefab, casePanel);
            _case.GetComponent<Image>().color = cases[index].colorRarity;
            _case.transform.Find("Item").GetComponent<Image>().sprite = cases[index].caseSprite;
            _case.GetComponent<CaseHolder>().indexRarity = cases[index].indexRarity;
            _case.GetComponent<CaseHolder>()._name = cases[index].name;
            _case.GetComponent<CaseHolder>().sprite = cases[index].caseSprite;
            _case.GetComponent<CaseHolder>().spriteOp = cases[index].caseSpriteOpen;
        }
    }

    IEnumerator Prize()
    {
        GameObject chest = prizePanel.Find("ImageSkin").gameObject;
        chest.GetComponent<Button>().interactable = false;
        Vector2 pos = chest.transform.localPosition;
        for (int i = 0; i < 50; i++)
        {
            float x = pos.x + Random.value*4;
            float y = pos.y + Random.value*3;
            chest.transform.localPosition = new Vector2(x, y);
            yield return new WaitForSeconds(0.05f);
        }
        chest.transform.localPosition = pos;
        chest.GetComponent<Image>().sprite = result.spriteOp;
        prizePanel.Find("Effect").gameObject.SetActive(true);
        result = null;

    }

    void GetNewSkin()
    {

    }

    public void GetPrize()
    {
        StartCoroutine(Prize());
        GetNewSkin();
    }

    void ShowPrizePanel()
    {
        isPrize = true;
        prizePanel.gameObject.SetActive(true);
        prizePanel.Find("skinName").GetComponent<Text>().text = result._name;
        prizePanel.Find("Rarity").GetComponent<Text>().text = "chest";
        prizePanel.Find("ImageSkin").GetComponent<Image>().sprite = result.sprite;
        prizePanel.Find("ImageSkin").GetComponent<Button>().interactable = true;
        for (int i = casePanel.childCount - 1; i >= 0; i--)
        {
            Destroy(casePanel.GetChild(i).gameObject);
        }
    }


    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (isPrize)
            {
                isPrize = false;
                prizePanel.Find("Effect").gameObject.SetActive(false);
                prizePanel.gameObject.SetActive(false);
                
            }
        }
        if (isRoll)
        {
            velocity = Mathf.MoveTowards(velocity, 0, Time.deltaTime * speedRoll);
            casePanel.transform.Translate(new Vector2(velocity, 0) * Time.deltaTime / 5);
            if (velocity == 0)
            {
                isRoll = false;
                RaycastHit2D hit = Physics2D.Raycast(center.position, Vector3.forward);

                if (hit)
                {
                    if (hit.collider.tag == "Finish")
                    {
                        result = hit.collider.GetComponent<CaseHolder>();
                        ShowPrizePanel();
                    }
                    else
                    {
                        velocity = -2f;
                        isRoll = true;
                    }
                }

            }
        }
    }
}
