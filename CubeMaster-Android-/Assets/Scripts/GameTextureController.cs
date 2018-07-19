using UnityEngine;
using System.IO;

public class GameTextureController : MonoBehaviour
{
    public Texture2D PlatformNormal;
    public Texture2D PlatformsTexture;
    public Vector2 tilingPlatform = Vector2.one;
    public Material[] PlatformsMaterial;
    public Material CubeMaterial;
    public GameObject skinsObj;
    SkinsHolder skinsHolder;

    [ContextMenu("Texture")]
    private void Start()
    {
        skinsHolder = skinsObj.GetComponent<SkinsHolder>();

        foreach (Material m in PlatformsMaterial)
        {
            if (PlatformsTexture)
            {
                m.mainTexture = PlatformsTexture;
                m.mainTextureScale = tilingPlatform;
            }         

            if (PlatformNormal)
            {
                m.SetTexture("_BumpMap", PlatformNormal);
                m.SetTextureScale("_BumpMap", tilingPlatform);
            }
            else
                m.SetTexture("_BumpMap", null);            
        }

        SetMaterial();
    }

    Skin TakeSkinFromPacks(Skin.Rarity rarity, string name)
    {
        Skin skin;
        switch (rarity)
        {
            case Skin.Rarity.Common:
                skin = skinsHolder.skinPacks[0].skins.Find(x => x.name == name);
                return skin;

            case Skin.Rarity.Uncommon:
                skin = skinsHolder.skinPacks[1].skins.Find(x => x.name == name);
                return skin;

            case Skin.Rarity.Golden:
                skin = skinsHolder.skinPacks[2].skins.Find(x => x.name == name);
                return skin;
        }

        return null;
    }


    void SetMaterial()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv"))
        {
            string[] info = File.ReadAllLines(Application.persistentDataPath + "/SaveData/" + "skinInfo.sv");
            Skin.Rarity _rarity = (Skin.Rarity)(System.Convert.ToInt32(info[0]));
            string _name = info[1];

            if (_name == "Stone")
            {
                CubeMaterial.SetTexture("_MainTex", Resources.Load<Skin>("Prefabs/Stone").sprite.texture);
                CubeMaterial.SetTexture("_BumpMap", Resources.Load<Skin>("Prefabs/Stone").normalSprite);
                return;
            }
            
            Skin skin = TakeSkinFromPacks(_rarity, _name);
            CubeMaterial.SetTexture("_MainTex", skin.sprite.texture);
            if (skin.normalSprite)
            {
                CubeMaterial.SetTexture("_BumpMap", skin.normalSprite);
            }
        }
        else
        {
            CubeMaterial.SetTexture("_MainTex", Resources.Load<Skin>("Prefabs/Stone").sprite.texture);
            CubeMaterial.SetTexture("_BumpMap", Resources.Load<Skin>("Prefabs/Stone").normalSprite);
        }

        Destroy(skinsObj);
        skinsHolder = null;
    }
}
