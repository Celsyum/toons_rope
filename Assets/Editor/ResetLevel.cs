using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class ResetLevel : Editor
{
    public override void OnInspectorGUI()
    {
        GameManager menu = (GameManager)target;
        menu.levelToLoad = EditorGUILayout.IntField("Level to load", menu.levelToLoad);


        if (GUILayout.Button("Reset Level"))
        {
            menu.ResetLevel();
        }
    }
}
