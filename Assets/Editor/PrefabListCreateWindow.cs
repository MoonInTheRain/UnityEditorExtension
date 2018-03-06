using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class PrefabListCreateWindow : EditorWindow {

    private const string PATH = "Assets/Prefabs/List.asset";

    private PrefabList list;

    [MenuItem("test/Prefab作成")]
    public static void ShowThisWindow()
    {
        GetWindow<PrefabListCreateWindow>("Prefab作成");
    }

    private void OnEnable()
    {
        list = AssetDatabase.LoadAssetAtPath<PrefabList>(PATH);
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("作成"))
            {
                Create();
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    private void Create()
    {
        if (list != null) { return; }

        list = ScriptableObject.CreateInstance<PrefabList>();

        AssetDatabase.CreateAsset(list, PATH);
    }
}
