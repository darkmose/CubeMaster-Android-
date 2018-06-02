﻿using UnityEngine;
using UnityEngine.UI;

public class Coin : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject.tag == "MainCube" || other.gameObject.tag == "SecondCube")
        {         
            SaveManager.coins++;
            GameObject.Find("GameHandler").GetComponent<GameHandler>().mplevel++;
            DestroyObject(this.gameObject);
            GameObject.Find("MainScreen").transform.Find("Coins").Find("Text").GetComponent<Text>().text = SaveManager.coins.ToString();

            SerializableVector vec = new SerializableVector
            {
                x = transform.parent.position.x,
                z = transform.parent.position.z
            };
            LevelManager.coinMaps.Add(vec);
        }

    }
}
