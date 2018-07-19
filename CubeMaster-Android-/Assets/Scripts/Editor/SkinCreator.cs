using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkinCreate))]
public class SkinCreator : Editor
{
    public override void OnInspectorGUI()
    {
        SkinCreate skinCreate = (SkinCreate)target;

        GUILayout.Label("Skins Creator");
        GUILayout.Space(5);
        if (GUILayout.Button("Reset Arrays"))
        {
            skinCreate.spriteArray = new Sprite[0];
            skinCreate.normalsArray = new Texture2D[0];
        }
        GUILayout.Space(10);
        base.OnInspectorGUI();

       

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);

        if (GUILayout.Button("Parse Skins"))
        {
            if (!skinCreate.isAutomatic)
            {
                if (skinCreate.spriteArray.Length != 0)
                {
                    skinCreate.Parse(skinCreate.type);
                }
                else
                {
                    Debug.Log("Array is empty");
                }
            }
            else
            {
                skinCreate.AutomaticParse();
            }            
        }

        GUILayout.Space(30);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Clear Skins"))
        {
            skinCreate.ClearSkinsNSkinPacks();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
    }
}
