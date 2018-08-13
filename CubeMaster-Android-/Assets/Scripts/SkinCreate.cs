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
    public bool isAutomatic = false;


    public enum skinType
    {
        Common,
        Uncommon,
        Golden
    }
    public skinType type = new skinType();

#if UNITY_EDITOR
    public void Parse(skinType ftype)
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
               CreateSkin(i,ftype);
            }
            Debug.Log("Parsing: " + (float)i / spriteArray.Length * 100 + " % \n");
        }

        Debug.Log("All Done Successfully\n");
    }

    public void AutomaticParse()
    {
#region paths //Initialize paths
        string[] pathsTexture = new string[3];
        pathsTexture[0] = "Assets/OtherTextures/SkinsTex/Common/Tex";
        pathsTexture[1] = "Assets/OtherTextures/SkinsTex/Uncommon/Tex";
        pathsTexture[2] = "Assets/OtherTextures/SkinsTex/Golden/Tex";
        string[] pathsNormals = new string[3];
        pathsNormals[0] = "Assets/OtherTextures/SkinsTex/Common/Normals";
        pathsNormals[1] = "Assets/OtherTextures/SkinsTex/Uncommon/Normals";
        pathsNormals[2] = "Assets/OtherTextures/SkinsTex/Golden/Normals";
#endregion

        for (int i = 0; i < 3; i++)
        {
            spriteArray = new Sprite[0];
            normalsArray = new Texture2D[0];

            TakeTextures(pathsTexture[i], "Texture");
            TakeTextures(pathsNormals[i], "Normals");

            Parse((skinType)i);
        }

    }


    void TakeTextures(string path, string type)
    {
      string[] paths = Directory.GetFiles(path);
        string[] temp;
        int minus = 0;

        for (int i = 0; i < paths.Length; i++)
        {
            if (paths[i].Contains(".meta"))
            {
                paths[i] = null;
                minus++;
            }
        }
        temp = new string[paths.Length - minus];
        int l = 0;

        for (int i = 0; i < paths.Length; i++)
        {
            if (paths[i] != null)
            {
                temp[l++] = paths[i];
            }
        }
        paths = null;
        paths = temp;

        switch (type)
        {
            case "Texture":

                spriteArray = new Sprite[paths.Length];
                int i = 0;

                foreach (string str in paths)
                {
                    spriteArray[i++] = AssetDatabase.LoadAssetAtPath<Sprite>(str);
                }

                break;

            case "Normals":
                
                normalsArray = new Texture2D[spriteArray.Length];
                int k = 0;

                foreach (string str in paths)
                {
                    while (true)
                    {
                        if (AssetDatabase.LoadAssetAtPath<Texture2D>(str).name == spriteArray[k].name+"N")
                        {
                            normalsArray[k++] = AssetDatabase.LoadAssetAtPath<Texture2D>(str);
                            break;
                        }
                        k++;
                    }
                }

                break;
        }       
    }


    private void CreateSkin(int index, SkinCreate.skinType _type)
    {
        Skin asset = ScriptableObject.CreateInstance<Skin>();
        string assetName = spriteArray[index].name;
        asset.name = assetName;
        asset.sprite = spriteArray[index];
        asset.rarity = (Skin.Rarity)_type;

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
        catch (System.NullReferenceException)
        {
            Debug.Log("Normals is null");
        }


        string path = "Assets/Skins/" + _type.ToString() + "/"+ assetName + ".asset";
        
        AssetDatabase.CreateAsset(asset, path);
        skinsHolder.skinPacks[(int)_type].skins.Add(asset);

        EditorUtility.SetDirty(skinsHolder.skinPacks[(int)_type]);
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