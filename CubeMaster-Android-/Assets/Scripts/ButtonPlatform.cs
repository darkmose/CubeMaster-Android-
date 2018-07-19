using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPlatform : MonoBehaviour
{
    MovePlatform movePlatform;
    MainCube cube;

    private void Start()
    {
        movePlatform = FindObjectOfType<MovePlatform>();   
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.3f);

        if (cube.IsVertical())
        {
            movePlatform.Action();
        }
        cube = null;

        yield break;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("MainCube") || other.gameObject.CompareTag("SecondCube"))
        {
            cube = FindObjectOfType<MainCube>();
            StartCoroutine(Wait());
        }
    }

}
