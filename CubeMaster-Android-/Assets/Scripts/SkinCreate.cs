using UnityEngine;
using System.IO;
using UnityEditor;

public class SkinCreate : MonoBehaviour
{
    [SerializeField]
    public Sprite[] spriteArray;
    [SerializeField]
    public Sprite[] normalsArray;
    public SkinsHolder skinsHolder;
   
    public enum skinType
    {
        Common,
        Uncommon,
        Golden
    }
    public skinType type = new skinType();


    public void Parse()
    {
        for (int i = 0; i < spriteArray.Length; i++)
        {
            if (spriteArray[i] != null)
            {
                if (File.Exists("Assets/Skins/" + type.ToString() + "/" + spriteArray[i].name + ".asset"))
                {
                    Debug.Log("Asset \'"+spriteArray[i].name+"\' is exist");
                    continue;
                }
               CreateSkin(i);
            }
            Debug.Log("Parsing: " + (float)i / spriteArray.Length * 100 + " % \n");
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("All Done Successfully\n");
    }

    private void CreateSkin(int index)
    {
        Skin asset = ScriptableObject.CreateInstance<Skin>();
        string assetName = spriteArray[index].name;
        asset.name = assetName;
        asset.sprite = spriteArray[index];
        asset.rarity = type.ToString();
        try
        {
            if (normalsArray[index] != null)
            {
                asset.normalSprite = normalsArray[index];
            }
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.Log("Normals is null");
        }


        string path = "Assets/Skins/" + type.ToString() + "/"+ assetName + ".asset";
        
        AssetDatabase.CreateAsset(asset, path);
        skinsHolder.skinPacks[(int)type].skins.Add(asset);
    }

}