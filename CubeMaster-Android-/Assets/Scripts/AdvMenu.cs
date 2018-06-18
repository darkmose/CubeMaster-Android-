using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

public class AdvMenu : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform panel;
    Vector2 _first;
    Vector2 _second;
    public GameObject gear;
    public Text lvlName;
    public GameHandler game;
    public Button share, retry, toMain;

    public string Name
    {
        get
        {
            return LevelManager.currentLevelName + "\n" + (LevelManager.currentIndexLocation).ToString() + "-" + LevelManager.currentLevel.ToString();
        }
    }

    bool isOpen = false;
        
    private void Start()
    {
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width);
        panel.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        panel.GetComponent<RectTransform>().localPosition = new Vector2(Screen.width, 0);
        gear.SetActive(false);
        lvlName.text = Name;

        share.onClick.RemoveAllListeners();
        retry.onClick.RemoveAllListeners();
        toMain.onClick.RemoveAllListeners();

        //share.onClick.AddListener(delegate {  })
        retry.onClick.AddListener(delegate { game.RetryLevel(); });
        toMain.onClick.AddListener(delegate { game.ToMainMenu(); });
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _first = eventData.position;
        gear.SetActive(true);
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 target = Input.mousePosition;
        target.y = panel.GetComponent<RectTransform>().anchoredPosition.y;
        panel.GetComponent<RectTransform>().localPosition = target;
    }
    public void AnimatePanel(string state)
    {
        switch (state)
        {
            case "Open":
                StartCoroutine(Move(new Vector2(0, panel.GetComponent<RectTransform>().localPosition.y)));
                break;

            case "Close":
                StartCoroutine(Move(new Vector2(Screen.width, panel.GetComponent<RectTransform>().localPosition.y)));
                break;
        }
    }
    IEnumerator Move(Vector2 target)
    {
        Vector2 temp = panel.GetComponent<RectTransform>().localPosition;
        while (panel.localPosition.x != target.x)
        {
            temp = Vector2.MoveTowards(panel.localPosition, target, 45);
            temp.x = Mathf.Clamp(temp.x, 0, target.x);
            panel.GetComponent<RectTransform>().localPosition = temp;

            yield return null;
        }
        if (!isOpen)
        {
            gear.SetActive(false);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        _second = eventData.position;
        float dist = Mathf.Abs(_second.x - _first.x);
        float value = Screen.width / 100 * 20;

        if (dist > value)
        {
            if (isOpen)
            {
                Time.timeScale = 1;
                isOpen = false;
                AnimatePanel("Close");
            }
            else
            {
                isOpen = true;
                AnimatePanel("Open");
                Time.timeScale = 0;
            }
        }
        else
        {
            if (isOpen)
            {
                isOpen = true;
                AnimatePanel("Open");
            }
            else
            {
                isOpen = false;
                AnimatePanel("Close");
            }
        }

    }
}
