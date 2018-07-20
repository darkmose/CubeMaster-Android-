using UnityEngine;
using System.Collections;

public class MovePlatformC : MonoBehaviour
{
    GameObject[] points;
    bool isOnPlatform = false;
    public float speed = 3f;
    Transform root;
    bool isMove;



    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MainCube") || other.CompareTag("SecondCube"))
        {
            StartCoroutine(wait());            
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("MainCube") || other.CompareTag("SecondCube"))
        {
            if (!isMove)
            {
                isOnPlatform = false;
            }
            
        }
    }

    IEnumerator wait()
    {        
        yield return new WaitForSeconds(0.3f);

        if (GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().IsVertical())
        {
            isOnPlatform = true;
        }

        yield break;
    }


    private void Start()
    {
        root = transform.parent.parent.parent;
        points = new GameObject[GameObject.FindGameObjectsWithTag("PlatformPoint").Length];
        for (int i = 0; i < points.Length; i++)
        {
            points[i] = GameObject.Find("PointPlatform" + (i+1).ToString()+"(Clone)");
        }

        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        byte x = 0;
        byte prev = (byte)(points.Length-1);

        while (true)
        {           
            Vector3 point = new Vector3(points[x].transform.position.x, this.transform.position.y, points[x].transform.position.z);

            points[prev].GetComponent<BoxCollider>().enabled = true;


            if (isOnPlatform)
            {
                GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().canMove = false;
                GameObject.Find("MainCube").transform.SetParent(this.transform);
            }

            isMove = true;

            while (transform.position != point)
            {
                transform.position = Vector3.MoveTowards(transform.position, point, Time.deltaTime*speed);
                yield return null;
            }

            isMove = false;

            if (isOnPlatform)
            {
                GameObject.Find("MainCube").transform.SetParent(root);
                GameObject.FindGameObjectWithTag("MainCube").GetComponent<MainCube>().canMove = true;
            }

            points[x].GetComponent<BoxCollider>().enabled = false;

            prev = x;

            if (x == points.Length - 1)
            {
                x = 0;
            }
            else
            {
                x++;
            }

            yield return new WaitForSeconds(4);
        }
    }

}
