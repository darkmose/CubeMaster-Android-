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
        RefreshTarget();
        mainCamera = Camera.main.gameObject;
    }

    public void RefreshTarget()
    {
        cubeScript = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
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

            Vector3 target = (GameObject.FindGameObjectWithTag("MainCube").transform.position - mainCamera.transform.position).normalized;
            target.x = mainCamera.transform.position.x;
            target.z = mainCamera.transform.position.z;

            mainCamera.transform.position = Vector2.MoveTowards(mainCamera.transform.position, target * zoomDelta, Time.deltaTime * 25);
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
