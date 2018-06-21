using UnityEngine;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif
public class SkinCreate : MonoBehaviour
{
    [SerializeField]
    public Sprite[] spriteArray;
    [SerializeField]
    public Texture2D[] normalsArray;
    public SkinsHolder skinsHolder;
   
    public enum skinType
    {
        Common,
        Uncommon,
        Golden
    }
    public skinType type = new skinType();

#if UNITY_EDITOR
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

        Debug.Log("All Done Successfully\n");
    }

    private void CreateSkin(int index)
    {
        Skin asset = ScriptableObject.CreateInstance<Skin>();
        string assetName = spriteArray[index].name;
        asset.name = assetName;
        asset.sprite = spriteArray[index];
        asset.rarity = (Skin.Rarity)type;

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

        EditorUtility.SetDirty(skinsHolder.skinPacks[(int)type]);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void ClearInv()
    {
        FindObjectOfType<Shop>().inventorySkins.skins.Clear();
        EditorUtility.SetDirty(FindObjectOfType<Shop>().inventorySkins);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
    public void ClearSkinsNSkinPacks()
    {
        string[] paths1 = Directory.GetFiles("Assets/Skins/Common/");
        string[] paths2 = Directory.GetFiles("Assets/Skins/Uncommon/");
        string[] paths3 = Directory.GetFiles("Assets/Skins/Golden/");

        foreach (string p in paths1)
        {
            AssetDatabase.DeleteAsset(p);
        }
        foreach (string p in paths2)
        {
            AssetDatabase.DeleteAsset(p);
        }
        foreach (string p in paths3)
        {
            AssetDatabase.DeleteAsset(p);
        }

        for (int i = 0; i < 3; i++)
        {
            FindObjectOfType<SkinsHolder>().skinPacks[i].skins.Clear();
            EditorUtility.SetDirty(FindObjectOfType<SkinsHolder>().skinPacks[i]);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
       
    }

#endif
}