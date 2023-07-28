using System;
using UnityEditor;
using UnityEngine;

public class GameplayTagEditor : EditorWindow
{
    [MenuItem("Assets/Gameplay Tag")]
    public static void OpenEditorWindow()
    {
        GetWindow<GameplayTagEditor>("Gameplay Tag");
    }

    private void OnGUI()
    {
       
    }
}
