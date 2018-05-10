using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseLevelHandler : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{

    Vector2 swipeBegin;
    Vector2 swipeEnd;
    Vector2 swipeDelta;

    public GameStartController game;


    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Input.touchCount == 1)
        {
            swipeBegin = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        swipeEnd = eventData.position;
        swipeDelta = swipeEnd - swipeBegin;
        if (swipeDelta.magnitude > 30)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0)
                {
                    game.PrevTypeLevel();
                }
                else
                {
                    game.NextTypeLevel();
                }
            }
        }
    }
}
