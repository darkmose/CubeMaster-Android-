using UnityEngine;
using UnityEngine.EventSystems;

public class InvContainer : MonoBehaviour, IPointerClickHandler,IPointerDownHandler,IPointerUpHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SetTexture();
    }

    float _t = 0;

    public Skin skin;

    void SetTexture()
    {
        Shop shop = FindObjectOfType<Shop>();
        shop.material.SetTexture("_MainTex",skin.sprite.texture);
        if (skin.normalSprite)
        {
            FindObjectOfType<Shop>().material.SetTexture("_BumpMap", skin.normalSprite);
        }
        else
        {
            FindObjectOfType<Shop>().material.SetTexture("_BumpMap", null);
        }
        shop.finishSkin = skin;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _t = Time.time - _t;
        if (_t > 1)
        {
            if (skin.name != "Stone") 
            {
                GameObject.Find("Shop").GetComponent<Shop>().SellSkinPanel(skin, this.gameObject);
            }
            
        }
        _t = 0;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _t = Time.time;
    }

}
