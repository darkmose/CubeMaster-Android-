using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AdvMenu : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public Transform panel;
    Vector2 _first;
    Vector2 _second;
    bool isOpen=false;

    public void OnBeginDrag(PointerEventData eventData)
    {
        _first = eventData.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 target = Input.GetTouch(0).position;
        //Vector2 target = Input.mousePosition;
        target.x = panel.GetComponent<RectTransform>().anchoredPosition.x;
        panel.GetComponent<RectTransform>().anchoredPosition = target;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _second = eventData.position;
        Vector2 dist = _second - _first;
                
        if (dist.y < -30 && !isOpen)
        {
            isOpen = true;
            panel.GetComponent<Animator>().SetTrigger("Open");
            Time.timeScale = 0;
        }
        else if (dist.y > 30 && isOpen)
        {
            isOpen = false;
            panel.GetComponent<Animator>().SetTrigger("Close");
            Time.timeScale = 1;
        }
    }
}
