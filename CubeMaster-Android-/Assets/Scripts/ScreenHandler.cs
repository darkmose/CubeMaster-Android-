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
        mainCamera = Camera.main.gameObject;
    }

    public void RefreshTarget(MainCube script)
    {
        cubeScript = null;
        cubeScript = script;
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
            Vector3 target = new Vector3(
                mainCamera.transform.position.x , 
                Mathf.MoveTowards(transform.position.y, transform.position.y + zoomDelta, Time.deltaTime*5) , 
                mainCamera.transform.position.z);
            target.y = Mathf.Clamp(target.y, 2.5f, 5);

            mainCamera.transform.position = target;

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
        if (swipeDelta.magnitude > 25)
        {
            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                if (swipeDelta.x > 0 && cubeScript.canMove && !cubeScript.isRotate)
                {
                    cubeScript.isRotate = true;
                    cubeScript.canMove = false;

                    string side = "right";
                    cubeScript.Moving(side);
                }
                else if (swipeDelta.x < 0 && cubeScript.canMove && !cubeScript.isRotate)
                {
                    cubeScript.isRotate = true;
                    cubeScript.canMove = false;
                    string side = "left";
                    cubeScript.Moving(side);
                }
            }
            else
            {
                if (swipeDelta.y > 0 && cubeScript.canMove && !cubeScript.isRotate)
                {
                    cubeScript.isRotate = true;
                    cubeScript.canMove = false;
                    string side = "up";
                    cubeScript.Moving(side);
                }
                else if (swipeDelta.y < 0 && cubeScript.canMove && !cubeScript.isRotate)
                {
                    cubeScript.isRotate = true;
                    cubeScript.canMove = false;
                    string side = "down";
                    cubeScript.Moving(side);
                }
            }
        }        
    }


}
