using System.Collections;
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
    public SkinsHolder skinHolder;
    bool canClosePrize = false;
    public Shop shop;
    public Animator anim;

    public void StartRoll()
    {
        if (SaveManager.coins >= 10)
        {
            if (!isRoll && !isPrize)
            {
                GenerateCases();
                casePanel.position = center.position;
                speedRoll = (byte)Random.Range(5, 6.5f);
                velocity = Random.Range(-25f, -28f);
                StartCoroutine(Sound(velocity/speedRoll));
                isRoll = true;
                SaveManager.coins -= 10;
            }
        }
        else
        {
            anim.Play("coinsinfoRed");
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
            _case.GetComponent<CaseHolder>().rarity = (CaseHolder.Rarity)cases[index].rarity;
            _case.GetComponent<CaseHolder>().sprite = cases[index].caseSprite;
            _case.GetComponent<CaseHolder>().spriteOp = cases[index].caseSpriteOpen;
        }
    }

    IEnumerator Sound(float vel)
    {
        while(vel > 0)
        {
            shop.Sound("Roll");
            float t = 2 / Mathf.Abs(velocity);
            vel -= t;
            yield return new WaitForSeconds(t);
        }
    }

    IEnumerator Prize()
    {
        Skin newSkin = GenerateSkin();
        shop.inventory.skins.Add(newSkin);

        AudioSource audio = prizePanel.GetComponent<AudioSource>();

        GameObject chest = prizePanel.Find("ImageSkin").gameObject;
        chest.GetComponent<Button>().interactable = false;
        Vector2 pos = chest.transform.localPosition;
        for (int i = 0; i < 50; i++)
        {
            float x = pos.x + Random.value * 4;
            float y = pos.y + Random.value * 3;
            chest.transform.localPosition = new Vector2(x, y);
            yield return new WaitForSeconds(0.05f);
        }
        chest.transform.localPosition = pos;
        chest.GetComponent<Image>().sprite = result.spriteOp;
        audio.Play();
        

        yield return new WaitForSeconds(1);
        prizePanel.Find("Effect").gameObject.SetActive(true);

        float scale = chest.transform.localScale.x;

        for (; chest.transform.localScale.x > 0 ;)
        {
            chest.transform.localScale -= Vector3.one * Time.deltaTime*10;
            yield return new WaitForFixedUpdate();
        }
        chest.GetComponent<Image>().sprite = newSkin.sprite;

        for (; chest.transform.localScale.x < scale ;)
        {
            chest.transform.localScale += Vector3.one * Time.deltaTime*10;
            yield return new WaitForFixedUpdate();
        }
        prizePanel.Find("Rarity").GetComponent<Text>().text = newSkin.rarity.ToString();
        prizePanel.Find("skinName").GetComponent<Text>().text = newSkin.name;

        chest.GetComponent<Button>().interactable = true;
        canClosePrize = true;

        result = null;

    }

    private Skin Randomizer(int index)
    {
        int count = skinHolder.skinPacks[index].skins.Count;
        return skinHolder.skinPacks[index].skins[Random.Range(0, count - 1)];
    }

    private Skin GenerateSkin()
    {
        Skin skin;
        

        switch (result.rarity)
        {
            case CaseHolder.Rarity.Common:
                skin = Randomizer(0);
                return skin;
                
            case CaseHolder.Rarity.Uncommon:
                skin = Randomizer(1);
                return skin;
                
            case CaseHolder.Rarity.Golden:
                skin = Randomizer(2);
                return skin;                
        }

        return null;        
    }

    public void GetPrize()
    {
        if (canClosePrize)
        {
            canClosePrize = false;
            isPrize = false;
            prizePanel.Find("Effect").gameObject.SetActive(false);
            prizePanel.gameObject.SetActive(false);
            return;
        }
        StartCoroutine(Prize());
    }

    void ShowPrizePanel()
    {
        isPrize = true;
        prizePanel.gameObject.SetActive(true);
        prizePanel.Find("Rarity").GetComponent<Text>().text = result.rarity.ToString();
        prizePanel.Find("skinName").GetComponent<Text>().text = "chest";
        prizePanel.Find("ImageSkin").GetComponent<Image>().sprite = result.sprite;
        prizePanel.Find("ImageSkin").GetComponent<Button>().interactable = true;
        for (int i = casePanel.childCount - 1; i >= 0; i--)
        {
            Destroy(casePanel.GetChild(i).gameObject);
        }
    }


    void Update()
    {
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
                        StopAllCoroutines();
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
