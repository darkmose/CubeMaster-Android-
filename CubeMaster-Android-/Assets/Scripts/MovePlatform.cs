using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour {

    Vector3 pointToMove;
    Vector3 startPosition;
    MainCube mainCube;


	void Start ()
    {
        transform.parent.GetComponent<BoxCollider>().enabled = false;
        mainCube = GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>();
        pointToMove = GameObject.FindGameObjectWithTag("PlatformPoint").transform.position;
        startPosition = this.transform.position;
        GameHandler gameHandler = GameObject.Find("GameHandler").GetComponent<GameHandler>();
        gameHandler.OnRetryLevel += GameHandler_OnRetryLevel;
    }

    private void GameHandler_OnRetryLevel(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        FromPoint();
    }

    IEnumerator Move(Vector3 point)
    {
        mainCube.canMove = false;

        while (transform.position != point)
        {             
            this.transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime * 10);
            yield return null;
        }

        if (point == startPosition)
        {
            GameObject.FindGameObjectWithTag("PlatformPoint").GetComponent<BoxCollider>().enabled = true;
            transform.parent.GetComponent<BoxCollider>().enabled = false;
        }
        else
        {
            GameObject.FindGameObjectWithTag("PlatformPoint").GetComponent<BoxCollider>().enabled = false;
            transform.parent.GetComponent<BoxCollider>().enabled = true;
        }
        
        mainCube.canMove = true;
    }

    void ToPoint()
    {
        StopAllCoroutines();
        StartCoroutine(Move(new Vector3(pointToMove.x, this.transform.position.y, pointToMove.z)));
    }
    void FromPoint()
    {
        StopAllCoroutines();
        StartCoroutine(Move(startPosition));
    }


    public void Action()
    {
        if ((int)transform.position.x == (int)pointToMove.x && (int)transform.position.z == (int)pointToMove.z)
        {          
            FromPoint();
        }
        else
        {
            ToPoint();
        }
    }

	
}
