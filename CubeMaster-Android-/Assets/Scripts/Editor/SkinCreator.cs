using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SkinCreate))]
public class SkinCreator : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SkinCreate skinCreate = (SkinCreate)target;

        GUILayout.Space(20);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);

        if (GUILayout.Button("Parse Skins"))
        {
            if (skinCreate.spriteArray.Length != 0)
            {
                skinCreate.Parse();
            }
            else
            {
                Debug.Log("Array is empty");
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
        if (GUILayout.Button("Clear Inventory"))
        {
            skinCreate.ClearInv();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(30);
    }
}
