using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenCase : MonoBehaviour {

    public Case[] cases;
    bool isRoll = false;
    public GameObject casePrefab;
    public Transform casePanel;
    byte speedRoll = 3;
    float velocity;
    public Transform center;
    [HideInInspector]
    public Case result;

    public void StartRoll()
    {
        if (!isRoll)
        {
            speedRoll = (byte)Random.Range(4, 6);
            velocity = Random.Range(-30f, -50f);
            isRoll = true;
        }        
    }

    void GenerateCases()
    {
        for (int i = 0; i < 50; i++)
        {
            int rand = Random.Range(0, 1000);
            byte index = 0;
            if (rand <= 600)
            {
                index = 1;
            }
            else if (rand > 600 && rand <= 800)
            {
                index = 2;
            }
            else
            {
                index = 3;
            }

            GameObject _case = Instantiate(casePrefab, casePanel);
            _case.GetComponent<Image>().color = cases[index].colorRarity;
            //_case.transform.Find("Item").GetComponent<Image>().sprite = cases[index].caseSprite;
            _case.GetComponent<CaseHolder>().indexRarity = cases[index].indexRarity;
            _case.GetComponent<CaseHolder>()._name = cases[index].name;
            _case.GetComponent<CaseHolder>().sprite = cases[index].caseSprite;
        }
        

    }

	void Update ()
    {
        if (isRoll)
        {
            velocity = Mathf.MoveTowards(velocity, 0, Time.deltaTime * speedRoll);
            casePanel.transform.Translate(new Vector2(velocity, 0) * Time.deltaTime);
            if (velocity == 0)
            {
                isRoll = false;
                RaycastHit2D hit = Physics2D.Raycast(center.position, Vector3.forward);
                if (hit.collider.tag == "Finish")
                {
                    
                }
                else
                {
                    velocity = -5f;
                    isRoll = true;
                }
            }
        }
    }
}
