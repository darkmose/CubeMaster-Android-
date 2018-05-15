using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(leveEdit))]
public class LevelEditor : Editor {

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        leveEdit lev = (leveEdit)target;

        GUILayout.Space(15);


        GUILayout.BeginHorizontal();
        GUILayout.Space(40);
        if (GUILayout.Button("Create a Level"))
        {
            lev.CreateLevel();
        }
        GUILayout.Space(40);
        GUILayout.EndHorizontal();
        GUILayout.Space(30);
    }

}
