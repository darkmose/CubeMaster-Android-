using UnityEngine;

public class leveEdit : MonoBehaviour
{

    public ColorPrefab[] colorPrefabs;
    public Texture2D sample;

    public void CreateLevel()
    {

        Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/NewLevel"),new Vector3(-10.19f, -0.16f, -11.07f),Quaternion.identity);

        for (int i = 0; i < sample.width; i++)
        {
            for (int j = 0; j < sample.height; j++)
            {
                ColorToPrefab(i, j);
            }
        }
        if (GameObject.FindWithTag("Start"))
        {
            GameObject a = Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/MainCube"), GameObject.FindWithTag("Start").transform.position+transform.up*0.65f, Quaternion.identity, GameObject.Find("NewLevel(Clone)").transform);
            a.name = "MainCube";
        }        
    }

    void ColorToPrefab(int x, int y)
    {
        if (sample)
        {
            Color pixel = sample.GetPixel(x, y);

            if (pixel.a == 0)
            {
                return;
            }
            else
            {
                foreach (ColorPrefab ColPref in colorPrefabs)
                {
                    if (pixel.Equals(ColPref.color))
                    {
                        Vector3 pos = new Vector3(x, 0, y);
                        GameObject a = Instantiate<GameObject>(ColPref.prefab, GameObject.Find("NewLevel(Clone)").transform.Find("Ground"));
                        a.transform.localPosition = pos;

                        if (a.name.Equals("Block(Clone)"))
                        {
                            a.transform.SetParent(GameObject.Find("NewLevel(Clone)").transform.Find("Blocks"));
                            a.transform.localPosition += transform.up*0.663f;
                        }
                    }
                }             
            }
        }        
    }

}
