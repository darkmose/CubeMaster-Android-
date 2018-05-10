using UnityEngine;
using UnityEngine.EventSystems;

public class ScreenHandler : MonoBehaviour,IBeginDragHandler,IEndDragHandler, IDragHandler
{

    MainCube cubeScript;
    GameObject mainCamera;

    Vector2 swipeBegin;
    Vector2 swipeEnd;
    Vector2 swipeDelta;

    float distance = 0;
    float zoomDelta = 0;
    Vector2 finger1, finger2;

    private void Start()
    {
        cubeScript = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
        mainCamera = Camera.main.gameObject;
    }

    void Zoom()
    {
        if (Input.touchCount == 2)
        {
            finger1 = Input.GetTouch(0).position;
            finger2 = Input.GetTouch(1).position;

            if (distance == 0)
            {
                distance = Vector2.Distance(finger1, finger2);
            }

            zoomDelta = Vector2.Distance(finger1, finger2) - distance;

            mainCamera.transform.position = Vector2.MoveTowards(mainCamera.transform.position, (GameObject.FindGameObjectWithTag("MainCube").transform.position - mainCamera.transform.position).normalized * zoomDelta, Time.deltaTime * 10);

            distance = Vector2.Distance(finger1, finger2);
        }
        else
        {
           distance = 0;
        }
    }



    void Update()
    {
        Zoom();
    }

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
                if (swipeDelta.x > 0 && cubeScript.canMove)
                {
                    cubeScript.canMove = false;
                    print("Horizontal");
                    string side = "right";
                    cubeScript.Moving(side);
                }
                else if (swipeDelta.x < 0 && cubeScript.canMove)
                {
                    cubeScript.canMove = false;
                    print("Horizontal");
                    string side = "left";
                    cubeScript.Moving(side);
                }
            }
            else
            {
                if (swipeDelta.y > 0 && cubeScript.canMove)
                {
                    cubeScript.canMove = false;
                    print("Vertical");
                    string side = "up";
                    cubeScript.Moving(side);
                }
                else if (swipeDelta.y < 0 && cubeScript.canMove)
                {
                    cubeScript.canMove = false;
                    print("Vertical");
                    string side = "down";
                    cubeScript.Moving(side);
                }
            }
        }        
    }


}
