﻿using UnityEngine;
using UnityEngine.EventSystems;

public class InvContainer : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SetTexture();
    }

    public Skin skin;

    void SetTexture()
    {
        FindObjectOfType<Shop>().material.SetTexture("_MainTex",skin.sprite.texture);
        if (skin.normalSprite)
        {
            FindObjectOfType<Shop>().material.SetTexture("_BumpMap", skin.normalSprite);
        }
        else
        {
            FindObjectOfType<Shop>().material.SetTexture("_BumpMap", null);
        }
    }
}
