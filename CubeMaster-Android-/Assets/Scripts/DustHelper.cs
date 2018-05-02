using UnityEngine;

public class DustHelper : MonoBehaviour
{
    GameObject particle;

    private void Start()
    {
        particle = Resources.Load<GameObject>("Prefabs/Dust");
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Platform") || collision.CompareTag("Start") || collision.CompareTag("End"))
        {
            print("eee");
            Instantiate(particle,collision.transform.position+transform.up*0.3f,Quaternion.identity);
        }
    }
}
