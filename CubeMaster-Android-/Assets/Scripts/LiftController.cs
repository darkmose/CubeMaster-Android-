using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftController : MonoBehaviour {

    MainCube mainCube;
    public int height = 5;
    bool isBelow = true;
    bool canGo = true;
    bool retry = false;

    private void Start()
    {
        GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        gameHandler.OnRetryLevel += GameHandler_OnRetryLevel;
    }

    private void GameHandler_OnRetryLevel(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        retry = true;
        LiftDown();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCube") || other.CompareTag("SecondCube"))
        {
            if (canGo)
            {               
                mainCube = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
                StartCoroutine(wait());
            }          
        }
    }

    IEnumerator wait()
    {
        yield return new WaitForSeconds(0.3f);

        if (mainCube.IsVertical())
        {
            Action();
        }
    }


    void Action()
    {
        if (isBelow)
        {
            LiftUp();
        }
        else
        {
            LiftDown();
        }
    }



    IEnumerator Move(Vector3 point)
    {
        canGo = false;
        mainCube.canMove = false;

        if (!retry)
        {
            GameObject.Find("MainCube").transform.SetParent(transform);
        }
        


        while (transform.position != point)
        {
            transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime*5);
            yield return null;
        }

        GameObject.Find("MainCube").transform.SetParent(transform.parent.parent);

        mainCube.canMove = true;

        retry = false;
        yield return new WaitForSeconds(2);
        canGo = true;
    }

    void LiftUp()
    {
        StartCoroutine(Move(new Vector3(transform.position.x, transform.position.y + height, transform.position.z)));
        isBelow = false;
    }

    void LiftDown()
    {
        StartCoroutine(Move(new Vector3(transform.position.x, transform.position.y - height, transform.position.z)));
        isBelow = true;
    }
}
